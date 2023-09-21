using Backend.DataBase.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "user";
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Events.OnValidatePrincipal = async conext => { };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.Ugger();
    app.UseSwaseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:8080");
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img")),
    RequestPath = "/img"
});

using var scope = app.Services.CreateScope();

using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

app.Run();