using IdScanner.UI.Application.Extension;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Domain.Model;
using IdScanner.UI.Infrastructure.Extension;
using IdScanner.UI.Infrastructure.Provider;

var builder = WebApplication.CreateBuilder(args);
var globalClass = new GlobalClass();
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddApplicationService();
builder.Services.AddInfrastrucureService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
//builder.Services.AddCors(option => option.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins("https://drive.google.com").AllowAnyMethod()
//.AllowAnyHeader()));
builder.Services.AddCors(option =>
	option.AddPolicy("AllowSpecificOrigin", builder =>
		builder.WithOrigins("https://drive.google.com")
			   .AllowAnyMethod()
			   .AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseCors();
}
//builder.Services.AddAuthentication("Cookies")
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/Login/Login";       // where to redirect if not logged in
//        options.LogoutPath = "/Account/Logout";     // logout endpoint
//        options.AccessDeniedPath = "/Login/AccessDenied"; // if unauthorized
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//    });
//app.Use(async (context, next) =>
//{
//    // Read a specific cookie
//    var token = context.Request.Cookies["jwtToken"];
//    globalClass.Token = token;
//    //if(token != null)
//    //{
//    //    globalClass.Token = token;
//    //}
//    //else
//    //{
//    //    globalClass.Token = token;
//    //}
//    await next.Invoke();
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
