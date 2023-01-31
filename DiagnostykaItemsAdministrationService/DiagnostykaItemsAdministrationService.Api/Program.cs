using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItem;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(typeof(GetItemQuery));

builder.Services.AddDbContext<AppDbContext>(dbBuilder =>
    dbBuilder
        .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), options => options.CommandTimeout(120)
            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

using var appDbContext = builder.Services.BuildServiceProvider().GetRequiredService<AppDbContext>();
appDbContext.Database.Migrate();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();