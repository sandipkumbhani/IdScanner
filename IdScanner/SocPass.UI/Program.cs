using Microsoft.AspNetCore.Authentication.Cookies;
using SocPass.UI.Application.Extension;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using SocPass.UI.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);
var globalClass = new GlobalClass();

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(AuthorizeTokenAttribute));
});

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
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    var globalClass = context.RequestServices.GetRequiredService<GlobalClass>();
    var token = context.Request.Cookies["jwtToken"];
    globalClass.Token = token;
    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    //await next();
    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
//app.Use(async (context, next) =>
//{
//    var token = context.Session.GetString("Token");

//    var path = context.Request.Path.Value?.ToLower();

//    // Allow unauthenticated access to these endpoints
//    bool isLoginPage = path.Contains("/login/login");
//    bool isLogoutPage = path.Contains("/login/logout");
//    bool isStaticFile = path.Contains("/css") || path.Contains("/js") || path.Contains("/images");

//    if (string.IsNullOrEmpty(token) && !isLoginPage && !isLogoutPage && !isStaticFile && path != "/")
//    {
//        context.Response.Redirect("/Login/Login");
//        return;
//    }

//    await next();
//});
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
