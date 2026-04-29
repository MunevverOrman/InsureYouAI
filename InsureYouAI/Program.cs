using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Models;
using InsureYouAI.Services;
using InsureYouAINew.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddHttpClient("openai", c =>
{
    c.BaseAddress = new Uri("https://api.openai.com/");
});

builder.Services.AddDbContext<InsureContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<InsureContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<AIService>();

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.UseExceptionHandler("/Error/500");
//app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
