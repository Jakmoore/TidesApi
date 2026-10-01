using TideApi.Clients;
using TideApi.Config;
using TideApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(
    builder.Configuration
    .GetSection("OpenWaters")
    .Get<OpenWatersConfig>()!);

builder.Services.AddSingleton(
    builder.Configuration
    .GetSection("Location")
    .Get<LocationConfig>()!);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddTransient<ITideService, TideService>();
builder.Services.AddTransient<IOpenWatersClient, OpenWatersClient>();

var app = builder.Build();
app.MapControllers();
app.Run();
