using CoreProject.Services;
using Core.Middleware;
using CoreProject.interfaces;
using CoreProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

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
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
   c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks", Version = "v1" });
   c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
       In = ParameterLocation.Header,
       Description = "Please enter JWT with Bearer into field",
       Name = "Authorization",
       Type = SecuritySchemeType.ApiKey
   });
   c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
   });
});
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

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => Results.Redirect("/login.html"));
app.MapControllers();

app.Run();


