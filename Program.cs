using CoreProject.Services;
using Core.Middleware;
using CoreProject.interfaces;
using CoreProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Serilog; // error

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddSerilog();
// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console()
//     .CreateLogger();

// builder.Host.UseSerilog();

builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.TokenValidationParameters =
                        TokenService.GetTokenValidationParameters();
                });

builder.Services.AddAuthorization(c =>
{
    c.AddPolicy("ADMIN",
        policy => policy.RequireClaim("type", "ADMIN"));
    c.AddPolicy("USER",
        policy => policy.RequireClaim("type", "ADMIN", "USER"));
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped(typeof(ActiveUser)); 
builder.Services.AddScoped(typeof(JsonService<>));
builder.Services.AddItemsConst<Shoes>();
builder.Services.AddItemsConst<User>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ActiveUser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.MapScalarApiReference(options =>
    //     options.WithTheme(ScalarTheme.Mars)
    // );
}

// app.UseLog();
// app.UseError();
app.MapGet("/", () => Results.Redirect("/login.html"));

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();


