using IdScanner.UI.Application.Extension;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Infrastructure.Extension;
using IdScanner.UI.Infrastructure.Provider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationService();
builder.Services.AddHttpClient<ILoginRepository, LoginRepository>();
builder.Services.AddHttpClient<IForgotPasswordRepository, ForgotPasswordRepository>();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
