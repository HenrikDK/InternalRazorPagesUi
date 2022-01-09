var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();

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
        config.ClearProviders();
        config.AddSerilog();
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
