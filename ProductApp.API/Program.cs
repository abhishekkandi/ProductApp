using Microsoft.EntityFrameworkCore;
using ProductApp.Infrastructure.Data;
using ProductApp.Application.Interfaces;
using ProductApp.Application.Services;
using ProductApp.Infrastructure.Data.Repositories;
using Microsoft.OpenApi.Models;
using Serilog;
 
var builder = WebApplication.CreateBuilder(args);
 
// Configure Serilog for logging
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});
 
// Add services to the container.
builder.Services.AddControllers();
 
// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
// Register application services and repositories
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
 
// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});
 
// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
});
 
// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1"));
}
 
app.UseHttpsRedirection();
 
app.UseRouting();
 
app.UseCors("AllowAll");
 
app.UseAuthentication();
app.UseAuthorization();
 
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
 
app.Run();