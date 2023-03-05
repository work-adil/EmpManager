using EmpManager.Core.Services.CQRS.Commands.Employees;
using EmpManager.Core.Services.Validators.Employees;
using EmpManager.Core.Services.WebAPI.Extensions;
using EmpManager.Infrastructure.RelationalDb;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddDbServices()
    .AddBusinessServices()
    .AddMediatRServices()
    .AddMvc();

builder.Services.AddValidatorsFromAssemblyContaining<AddEmployeeValidator>()
    .AddFluentValidationAutoValidation();

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
