var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/ReverseProxy", "sp/{*url}");
        options.Conventions.AddPageRoute("/ReverseProxy", "back-office/sp/{*url}");
    });

builder.WebHost.UseUrls("http://*:80/");

var app = builder.Build();

app.UseStatusCodePages();
app.UseStatusCodePagesWithReExecute("/Status{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
