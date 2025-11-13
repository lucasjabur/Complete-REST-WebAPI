using REST_WebAPI.Configurations;
using REST_WebAPI.Files.Exporters.Factory;
using REST_WebAPI.Files.Exporters.Implementations;
using REST_WebAPI.Files.Importers.Factory;
using REST_WebAPI.Files.Importers.Implementations;
using REST_WebAPI.Hypermedia.Filters;
using REST_WebAPI.Mail;
using REST_WebAPI.Repositories;
using REST_WebAPI.Repositories.Implementations;
using REST_WebAPI.Services;
using REST_WebAPI.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLogging();

builder.Services.AddControllers(options => {
    options.Filters.Add<HypermediaFilter>();
}).AddContentNegotiation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenAPIConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddRouteConfig();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddHATEOASConfiguration();
builder.Services.AddEmailConfiguration(builder.Configuration);
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookServices, BookServicesImpl>();

builder.Services.AddScoped<PersonServicesImplV2>();

builder.Services.AddScoped<IEmailServices, EmailServicesImpl>();
builder.Services.AddScoped<EmailSender>();

builder.Services.AddScoped<CsvImporter>();
builder.Services.AddScoped<XlsxImporter>();
builder.Services.AddScoped<FileImporterFactory>();
builder.Services.AddScoped<CsvExporter>();
builder.Services.AddScoped<XlsxExporter>();
builder.Services.AddScoped<FileExporterFactory>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IFileServices, FileServicesImpl>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseCorsConfiguration(builder.Configuration);

app.MapControllers();
app.UseHATEOASRoutes();

app.UseSwaggerConfiguration();
app.UseScalarConfiguration();

app.Run();
