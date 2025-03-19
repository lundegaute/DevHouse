using DevHouse.Data;
using DevHouse.Models;
using DevHouse.Services;
using DevHouse.SwaggerExamples;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

#region Builder
var builder = WebApplication.CreateBuilder(args);

// Connecting to database
var connectionString = builder.Configuration.GetConnectionString("DevHouseDbConnection")
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

    // Adding XML comments to Swagger
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateRoleExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<UpdateRoleExample>();

//builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TeamService>();
//builder.Services.AddScoped<ProjectTypeService>();
builder.Services.AddScoped<DeveloperService>();
builder.Services.AddScoped<RoleService>();

#endregion

#region App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion