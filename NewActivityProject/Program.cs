using System;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Domain.Models; 
using Infrastructure.Data;
using Infrastructure.Services;
using NewActivityProject.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Domain.Interfaces;
using NewActivityProject.Security;
using Infrastructure.Repositories;
using FluentValidation;
using Domain.Validation;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Hubs;
using Infrastructure;
using CloudinaryDotNet;
using Domain.DTO;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/*var s = new BlobStorageService(builder.Configuration);
await s.ListBlobContainersAsync();*/

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173") 
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); 
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Infrastructure")));

var cloudinarySettings = builder.Configuration.GetSection("Cloudinary").Get<CloudinarySettings>();


builder.Services.AddSingleton<Cloudinary>(serviceProvider =>
{
    var account = new Account(
        cloudinarySettings.CloudName,
        cloudinarySettings.ApiKey,
        cloudinarySettings.ApiSecret
    );
    return new Cloudinary(account);
});

builder.Services.AddDefaultIdentity<ApplicationUser>() 
    .AddEntityFrameworkStores<AppDbContext>(); 

builder.Services.AddControllers();
builder.Services.AddTransient<SeedUsers>();
builder.Services.AddTransient<TokenService>();
//builder.Services.AddTransient<BlobStorageService>();
//builder.AddJwtAuthentication();
builder.Services.AddScoped<IAccessUser, AccessUser>();
//builder.Services.AddScoped<IActivityAttendeeRepository, ActivityAttendeeRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<IValidator<Activity>, ActivityValidator>();





builder.Services.AddApplication();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddSignalR();
builder.Host.UseSerilog((context, configuration) =>
   configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<SeedUsers>();
seeder.SeedDataAsync();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();

app.UseWebSockets();
app.MapHub<ChatHub>("/chat");
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();

public partial class Program { }