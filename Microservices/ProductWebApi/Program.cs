using Microsoft.EntityFrameworkCore;
using ProductWebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD");

var connection = $"server={dbHost};port=3306;database={dbName};user=root;password={dbPassword}";
var connect = "Server=(localdb)\\mssqllocaldb;Database=ProductWebApi;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlServer(connect));

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
