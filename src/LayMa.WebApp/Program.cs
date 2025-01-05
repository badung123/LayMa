using LayMa.WebApp.Constant;
using LayMa.WebApp.Extensions;
using LayMa.WebApp.Interface;
using LayMa.WebApp.Service;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
builder.Services.Configure<GoogleCaptchaOptions>(configuration.GetSection("GoogleCaptcha"));
builder.Services.AddSingleton<IRecaptchaExtension, RecaptchaExtension>();
builder.Services.AddHttpClient<ICaptchaService, GoogleCaptchaService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://www.google.com/");
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
