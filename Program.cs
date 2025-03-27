using DevHouse.Data;
using DevHouse.Models;
using DevHouse.Services;
using DevHouse.SwaggerExamples;
using DevHouse.JwtConfiguration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

#region Builder
var builder = WebApplication.CreateBuilder(args);

// Connecting to database
var connectionString = builder.Configuration.GetConnectionString("AZURE_MYSQL_CONNECTIONSTRING")
    ?? builder.Configuration.GetConnectionString("DevHouseDbConnection") 
        ?? throw new InvalidOperationException("Connection string is missing");
builder.Services.AddDbContext<DataContext>(options => options.UseMySQL(connectionString));

// Add services to the container.
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { 
        Version = "V1", 
        Title = "Dev House",
        Description = "API to keep track of In-House development projects",
    });
    options.ExampleFilters();

    // Addig Jwt functionality to swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please insert a valid Jwt token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Adding XML comments to Swagger
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    options.IncludeXmlComments(xmlPath);
});

#region Jwt Authentication

var JwtSettings = new JwtSettings();
builder.Configuration.GetSection(nameof(JwtSettings)).Bind(JwtSettings);
builder.Services.AddSingleton(JwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtSettings.Issuer,
        ValidAudience = JwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes(JwtSettings.SecretKey))
    };
});
builder.Services.AddAuthorization();

#endregion

builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateRoleExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<UpdateRoleExample>();

builder.Services.AddScoped<ProjectTypeService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<DeveloperService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();

#endregion

#region App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion