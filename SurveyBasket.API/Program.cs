using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repositories;
using Serilog;
using Service;
using Service.Mapping;
using ServiceAbstraction;
using SurveyBasket.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json" , "v1"));
}

app.UseSerilogRequestLogging();


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();