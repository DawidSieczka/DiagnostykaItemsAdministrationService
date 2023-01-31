using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Common.Exceptions.Filters;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
using DiagnostykaItemsAdministrationService.Persistence;
using DiagnostykaItemsAdministrationService.Persistence.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IHashGenerator, HashGenerator>();
builder.Services.AddMediatR(typeof(GetItemByIdQuery));

builder.Services.AddDbContext<AppDbContext>(dbBuilder =>
    dbBuilder
        .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), options => options.CommandTimeout(120)
            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

using var appDbContext = builder.Services.BuildServiceProvider().GetRequiredService<AppDbContext>();
appDbContext.Database.Migrate();


builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Diagnostyka Items Administration Service API",
                Description = "The API is responsible for Items administration. Based on DDD and CQRS Architecture developed for microservices approach. Persistance of the application is based on MS SQL Server.",
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

builder.Services.AddControllers(options =>
        options.Filters.Add(new HttpResponseExceptionFilter(builder.Environment))
    ).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateItemCommandValidator>());

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

                var responseBody = app.Environment.IsDevelopment() ?
                                      new BaseExceptionModel(context.Response.StatusCode, exception.Message, exception.StackTrace, exception.InnerException?.Message) :
                                      new BaseExceptionModel(context.Response.StatusCode, "Server error");

                await context.Response.WriteAsJsonAsync(responseBody);
            }
        }
    });
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
    context?.Seed();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();