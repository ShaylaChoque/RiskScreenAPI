using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RiskScreenAPI.Security.Authorization.Handlers.Implementations;
using RiskScreenAPI.Security.Authorization.Handlers.Interfaces;
using RiskScreenAPI.Security.Authorization.Middleware;
using RiskScreenAPI.Security.Authorization.Settings;
using RiskScreenAPI.Security.Domain.Repositories;
using RiskScreenAPI.Security.Domain.Services;
using RiskScreenAPI.Security.Persistence.Repositories;
using RiskScreenAPI.Security.Services;
using RiskScreenAPI.Shared.Persistence.Contexts;
using RiskScreenAPI.Shared.Persistence.Repositories;
using RiskScreenAPI.WebScraping.Domain.Service;
using RiskScreenAPI.WebScraping.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add API Documentation Info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RiskScreen API",
        Version = "v1",
        Description = "RiskScreen API Documentation"
    });
    options.EnableAnnotations();
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            Array.Empty<string>()
        }
    });
});

// Authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Secret"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            options.UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
    });

// Add lowercase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Inject shared services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AppSettings Configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


// Inject web scraping services
builder.Services.AddScoped<IOffshoreEntityService, OffshoreEntityService>();

// Inject security services
builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Automapper configuration
builder.Services.AddAutoMapper(
    typeof(RiskScreenAPI.Security.Mapping.ModelToResourceProfile),
    typeof(RiskScreenAPI.Security.Mapping.ResourceToModelProfile));

var app = builder.Build();

// Validation for ensuring database objects are created
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<AppDbContext>())
{
    context?.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    { 
        options.SwaggerEndpoint("v1/swagger.json", "v1"); 
        options.RoutePrefix = "swagger";
    });
}

//Configure CORS
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure Error Handler Middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure JWT Handling
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();