using DevHouse.Data;
using DevHouse.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

#region Builder
var builder = WebApplication.CreateBuilder(args);

// Connecting to database
var connectionString = builder.Configuration.GetConnectionString("DevHouseDbConnection")
    ?? throw new InvalidOperationException("Connection string is missing");
builder.Services.AddDbContext<DataContext>(options => options.UseMySQL(connectionString));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { 
        Version = "V1", 
        Title = "Dev House",
        Description = "API to keep track of In-House development projects",
    });

    // Adding XML comments to Swagger
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    options.IncludeXmlComments(xmlPath);
});
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