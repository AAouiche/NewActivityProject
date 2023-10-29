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



var builder = WebApplication.CreateBuilder(args);

/*var s = new BlobStorageService(builder.Configuration);
await s.ListBlobContainersAsync();*/


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Infrastructure")));


builder.Services.AddDefaultIdentity<ApplicationUser>() 
    .AddEntityFrameworkStores<AppDbContext>(); 

builder.Services.AddControllers();
builder.Services.AddTransient<SeedUsers>();
builder.Services.AddTransient<TokenService>();
//builder.Services.AddTransient<BlobStorageService>();
builder.AddJwtAuthentication();
builder.Services.AddScoped<IAccessUser, AccessUser>();
//builder.Services.AddScoped<IActivityAttendeeRepository, ActivityAttendeeRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        }) ;
});

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


app.UseRouting();
app.UseCors();
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();