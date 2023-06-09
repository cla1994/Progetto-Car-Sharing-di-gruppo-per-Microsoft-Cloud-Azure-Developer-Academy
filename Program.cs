using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Academy2023.Net.Models;
using Microsoft.EntityFrameworkCore;
using Academy2023.Net.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

string cnString = builder.Configuration.GetConnectionString("sqlite");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(cnString));
/*
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd")); 
/*/
builder.Services.AddControllersWithViews(options =>
//{ });
{
  var policy = new AuthorizationPolicyBuilder()
       .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddControllersAsServices();

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI(); 

builder.Services.AddHttpClient("routefinder_routes", client => {
    client.BaseAddress = new Uri(builder.Configuration["routefinder:routes"]); // endpoint is retreived by the config file
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");
    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
    client.DefaultRequestHeaders.Add("X-Goog-Api-Key", "AIzaSyAyfiXOt2c92VbTeeErK6fsDTSlpMwttIY");
    client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.duration,routes.distanceMeters");
});

builder.Services.AddHttpClient("routefinder_distanceMatrix", client => {
    client.BaseAddress = new Uri(builder.Configuration["routefinder:distanceMatrix"]); // endpoint is retreived by the config file
   // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");
    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
    client.DefaultRequestHeaders.Add("X-Goog-Api-Key", "AIzaSyAyfiXOt2c92VbTeeErK6fsDTSlpMwttIY");
    client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "originIndex,destinationIndex,duration,distanceMeters,status,condition");
});

builder.Services.AddHttpClient("routefinder_geoCoding", client => {
    client.BaseAddress = new Uri(builder.Configuration["routefinder:geoCoding"]); // endpoint is retreived by the config file
                                                                                       // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");
                                                                                       //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
    client.DefaultRequestHeaders.Add("X-Goog-Api-Key", "AIzaSyAyfiXOt2c92VbTeeErK6fsDTSlpMwttIY");
    client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "geometry");
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
