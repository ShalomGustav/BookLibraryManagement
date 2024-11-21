using BookLibraryManagement.Middlewares;
using BookLibraryManagement.Repositories;
using BookLibraryManagement.Services;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookDbContext>(options => 
    options.UseSqlServer("Data Source=(local);Initial Catalog=BookLibrary;Persist Security Info=True;User ID=test;Password=test;MultipleActiveResultSets=True;Connect Timeout=30;TrustServerCertificate=True"));

builder.Services.AddScoped<BookServices>();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));//регистрация для сборки

builder.Services.AddFluentValidationAutoValidation(); //проверить зависимости 
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();


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
