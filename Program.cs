using CoreProject.Services;
using Core.Middleware;
using CoreProject.interfaces;
using CoreProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

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
    c.AddPolicy("Admin",
        policy => policy.RequireClaim("type", "Admin"));
    c.AddPolicy("User",
        policy => policy.RequireClaim("type", "Admin", "User"));
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped(typeof(ActiveUser)); 
builder.Services.AddScoped(typeof(JsonService<>));
builder.Services.AddItemsConst<Shoes>();
builder.Services.AddItemsConst<User>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.MapGet("/", () => Results.Redirect("/index.html"));

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();

