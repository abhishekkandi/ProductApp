
using ProductApp.API.Extensions;
 
var builder = WebApplication.CreateBuilder(args);

builder.ConfigureHost();

builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

app.ConfigureApp();
app.Run();