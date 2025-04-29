using CoreProject.Services;
using Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddShoesConst();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(c =>
{
    c.AddPolicy("Admin",
        policy => policy.RequireClaim("type", "Admin"));
    c.AddPolicy("User",
        policy => policy.RequireClaim("type", "Admin", "User"));
});

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

app.MapControllers();

app.Run();

