using CachePractice.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();