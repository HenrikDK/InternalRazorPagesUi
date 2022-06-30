var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLamar((context, registry) =>
{
    registry.Scan(x =>
    {
        x.AssemblyContainingType<Program>();
        x.WithDefaultConventions();
        x.LookForRegistries();
    });
});

builder.WebHost
    .ConfigureKestrel(x => x.ListenAnyIP(8080))
    .ConfigureLogging((context, config) =>
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(new JsonFormatter(renderMessage: true))
            .CreateLogger();
        
        config.ClearProviders();
        config.AddSerilog(logger);
    });

builder.Services.AddMemoryCache();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/ReverseProxy", "sp/{*url}");
    options.Conventions.AddPageRoute("/ReverseProxy", "back-office/sp/{*url}");
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

var app = builder.Build();

app.UseStatusCodePages();
app.UseStatusCodePagesWithReExecute("/Status{0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapMetrics();

app.Run();

Log.CloseAndFlush();
