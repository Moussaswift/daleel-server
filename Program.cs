using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Services;
using Npgsql;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure DbContext with increased command timeout
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        npgsqlOptionsAction: sqlOptions =>
        {
            sqlOptions.CommandTimeout(120); // Increase timeout to 120 seconds
        }));

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Daleel API", Version = "v1" });
});

builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ISourceService, SourceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Daleel API v1");
        c.RoutePrefix = "api-docs"; // Set custom route for Swagger UI
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
