using InternalRazorPagesUi;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;

var builder = WebHost.CreateDefaultBuilder(args);

var host = builder
    .UseKestrel()
    .UseLamar()
    .UseUrls("http://*:80/")
    .UseStartup<Startup>()
    .Build();

host.Run();
