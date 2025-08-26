using Emertec.UI.Application.Extension;
using Google.Apis.Drive.v3;
using IdScanner.Infrastructure.Data;
using IdScanner.Infrastructure.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//database connection string
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(sp =>
{
    var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "GoogleDriveKeys", "client_secret_450198704638-s68uhit4jk57hqpdj7tgpq7jhrq75qca.apps.googleusercontent.com.json");

    return new GoogleDriveService(
        new[] { DriveService.Scope.DriveFile }, 
        "Broadsys ID Scanner",                  
        credentialPath                       
    );
});


builder.Services.AddEfcoreInfrastrucureService();
builder.Services.AddApplicationService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
