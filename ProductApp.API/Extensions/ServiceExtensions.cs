
using Microsoft.EntityFrameworkCore;
using ProductApp.Infrastructure.Data;
using ProductApp.Application.Interfaces;
using ProductApp.Application.Services;
using ProductApp.Infrastructure.Data.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Reflection;
using ProductApp.Application.Validators;
using FluentValidation.AspNetCore;
using ProductApp.Application.Mapping;
using AutoMapper;
using ProductApp.API.Filters;
using Microsoft.AspNetCore.ResponseCompression;

namespace ProductApp.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers(options => {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ExecutionTimeFilter>();
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
            
            // Configure Entity Framework Core with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                , b => b.MigrationsAssembly("ProductApp.Infrastructure")));
            
            // Register application services and repositories
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.ConfigureAutoMapper();
            
            // Configure API versioning
            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services.Configure<GzipCompressionProviderOptions>(options => 
                options.Level = System.IO.Compression.CompressionLevel.Fastest
            );
            services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            
            services.ConfigureSwagger();
            
            // Configure CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => 
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "ProductApp.Api.xml");
            // Configure Swagger/OpenAPI
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(filePath);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
            });
            
        }

    }    
}