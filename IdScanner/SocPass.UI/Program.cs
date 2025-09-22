using Microsoft.AspNetCore.Authentication.Cookies;
using SocPass.UI.Application.Extension;
using SocPass.UI.Domain.Model;
using SocPass.UI.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);
var globalClass = new GlobalClass();
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationService();
builder.Services.AddInfrastrucureService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<SocPass.UI.Domain.Model.GlobalClass>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Home/Denied";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
    });

builder.Services.AddAuthorization();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    var globalClass = context.RequestServices.GetRequiredService<GlobalClass>();
    var token = context.Request.Cookies["jwtToken"];
    globalClass.Token = token;
    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
