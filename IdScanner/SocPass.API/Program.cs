using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using SocPass.API.Jobs;
using SocPass.Application.Extension;
using SocPass.Domain.DTO;
using SocPass.Infrastructure.Data;
using SocPass.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddQuartz(q =>
{
    var QrCodeCleanupKey = new JobKey("QrCodeCleanupJob");
    q.AddJob<DeleteQr>(opts => opts.WithIdentity(QrCodeCleanupKey));
    var mp3ToRsaCron = builder.Configuration["Quartz:DeleteQr"];
    q.AddTrigger(opts => opts
        .ForJob(QrCodeCleanupKey)
        .WithIdentity("TriggerMP3ToRSA")
        .WithCronSchedule(mp3ToRsaCron, cron => cron.WithMisfireHandlingInstructionDoNothing()));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.Configure<AppSettingsDTO>(builder.Configuration.GetSection("AppSettings"));

//database connection string
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddApplicationService();
builder.Services.AddInfrastrucureService();
// Add services to the container.
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;
    //options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Access Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
