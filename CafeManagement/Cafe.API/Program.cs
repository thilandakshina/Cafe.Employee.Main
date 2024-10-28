using Cafe.Business.Commands.Cafe;
using Cafe.Business.Commands.Employee;
using Cafe.Data.Context;
using Cafe.Data.Repositories;
using Cafe.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Data Layer
builder.Services.AddDbContext<CafeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICafeRepository, CafeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICafeEmployeeRepository, CafeEmployeeRepository>();

// Business Layer - MediatR registration for both Cafe and Employee
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateCafeCommand).Assembly);    // Registers Cafe commands
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly); // Registers Employee commands
});

// AutoMapper Profiles registration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(CreateCafeCommand).Assembly);     // Register Cafe profiles
    cfg.AddMaps(typeof(CreateEmployeeCommand).Assembly); // Register Employee profiles
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000") // Your React app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
