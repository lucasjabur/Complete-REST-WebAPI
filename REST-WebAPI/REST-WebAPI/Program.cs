using REST_WebAPI.Configurations;
using REST_WebAPI.Repositories;
using REST_WebAPI.Repositories.Implementations;
using REST_WebAPI.Services;
using REST_WebAPI.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLogging();

builder.Services.AddControllers();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.AddScoped<IBookServices, BookServicesImpl>();
builder.Services.AddScoped<IPersonRepository, PersonRepositoryImpl>();
builder.Services.AddScoped<IBookRepository, BookRepositoryImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
