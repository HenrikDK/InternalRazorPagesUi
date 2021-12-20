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

builder.Services.AddRazorPages();

builder.WebHost.UseKestrel()
    .UseUrls("http://*:8080/");

var app = builder.Build();

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
