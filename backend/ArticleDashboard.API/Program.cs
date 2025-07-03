using ArticleDashboard.API.Mappings;
using ArticleDashboard.API.Middleware;
using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Domain.Interfaces;
using ArticleDashboard.Infrastructure.DataSeeder;
using ArticleDashboard.Infrastructure.Persistence;
using ArticleDashboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration));

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Swagger document info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Article Dashboard API",
        Version = "v1",
        Description = "Manage and filter bicycle components.",
        Contact = new OpenApiContact
        {
            Name = "NOCA mobility GmbH",
            Email = "dev@noca-mobility.com",
            Url = new Uri("https://www.noca-mobility.com/")
        }
    });
});

// EF Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(ArticleProfile).Assembly);

// Repository
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IBicycleCategoryRepository, BicycleCategoryRepository>();

// CQRS Handler Registrations
builder.Services.Scan(scan => scan
    .FromApplicationDependencies()
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // your Angular app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Migrate & Seed DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    await DbSeeder.SeedBicycleCategoriesAsync(db);
    await DbSeeder.SeedArticlesAsync(db);
}

// Middlewares : Swagger, Exception Handling
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
