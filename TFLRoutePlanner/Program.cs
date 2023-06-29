using TFLRoutePlanner;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

Log.Logger = (Serilog.ILogger)new LoggerConfiguration()
     .WriteTo.File(new JsonFormatter(),
        "important-logs.json",
        restrictedToMinimumLevel: LogEventLevel.Warning)
    .MinimumLevel.Debug()
    .CreateLogger();

var startup = new Startup(builder.Environment);
startup.ConfigureServices(builder.Services);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
startup.Configure(app, builder.Environment);

Log.CloseAndFlush();
