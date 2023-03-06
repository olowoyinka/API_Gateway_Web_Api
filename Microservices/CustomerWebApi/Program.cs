using CustomerWebApi;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

//var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
//var connection = @"Server=127.0.0.1,8001;Database=dms_customer;User=sa;Password=P@ssw0rd121#;";
var connect = "Server=(localdb)\\mssqllocaldb;Database=CustomerWebApi;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(connect));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();