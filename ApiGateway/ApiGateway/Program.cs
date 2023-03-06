// using JwtAuthenticationManager;
// using Ocelot.DependencyInjection;
// using Ocelot.Middleware;

// var builder = WebApplication.CreateBuilder(args);
// builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//     .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
//     .AddEnvironmentVariables();

// builder.Services.AddOcelot(builder.Configuration);
// builder.Services.AddCustomJwtAuthentication();

// var app = builder.Build();
// await app.UseOcelot();

// app.UseAuthentication();
// app.UseAuthorization();

// app.Run();


using ApiGateway.Config;
using JwtAuthenticationManager;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

var routes = "Routes";

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routes;
});

builder.Services.AddOcelot(builder.Configuration).AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseOcelot().Wait();

app.MapControllers();

app.Run();