using InternalRazorPagesUi.Infrastructure;
using Lamar.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseLamar((context, registry) =>
    {
        registry.Scan(x =>
        {
            x.AssemblyContainingType<Program>();
            x.WithDefaultConventions();
            x.LookForRegistries();
        });
        registry.AddMemoryCache();
    });

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/ReverseProxy", "sp/{*url}");
        options.Conventions.AddPageRoute("/ReverseProxy", "back-office/sp/{*url}");
    });

builder.WebHost.UseKestrel()
    .UseUrls("http://*:80/");

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

app.Run();
