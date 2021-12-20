using InternalRazorPagesUi.Infrastructure;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace InternalRazorPagesUi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLamar(new UiRegistry())
            .AddMemoryCache()
            .AddRazorPages()
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/ReverseProxy", "sp/{*url}");
                options.Conventions.AddPageRoute("/ReverseProxy", "back-office/sp/{*url}");
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });
        
        services.AddLogging(x =>
        {
            x.AddDebug();
            x.AddConsole();
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStatusCodePages();
        app.UseStatusCodePagesWithReExecute("/Status{0}");

        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}