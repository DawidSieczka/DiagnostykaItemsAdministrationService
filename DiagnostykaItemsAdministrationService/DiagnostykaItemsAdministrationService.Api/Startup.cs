using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Common.Exceptions.Filters;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
using DiagnostykaItemsAdministrationService.Persistence;
using DiagnostykaItemsAdministrationService.Persistence.Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace DiagnostykaItemsAdministrationService.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddTransient<IHashGenerator, HashGenerator>();
        services.AddMediatR(typeof(GetItemByIdQuery));

        services.AddDbContext<AppDbContext>(dbBuilder =>
            dbBuilder
                .UseSqlServer(_configuration.GetConnectionString("SqlServer"), options => options.CommandTimeout(120)
                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

        using var appDbContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Diagnostyka Items Administration Service API",
                Description = "The API is responsible for Items administration. Based on DDD and CQRS Architecture developed for microservices approach. Persistance of the application is based on MS SQL Server.",
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.ExampleFilters();
            options.OperationFilter<AddResponseHeadersFilter>();
        });
        services.AddSwaggerExamples();

        services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter(_env))
            ).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateItemCommandValidator>());

        services.AddRouting(options => options.LowercaseUrls = true);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(e =>
        {
            e.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature is not null)
                {
                    var exception = contextFeature.Error;
                    if (exception is not CustomException)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";

                        var responseBody = env.IsDevelopment() ?
                                              new BaseExceptionModel(context.Response.StatusCode, exception.Message, exception.StackTrace, exception.InnerException?.Message) :
                                              new BaseExceptionModel(context.Response.StatusCode, "Server error");

                        await context.Response.WriteAsJsonAsync(responseBody);
                    }
                }
            });
        });

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            context?.Seed();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}