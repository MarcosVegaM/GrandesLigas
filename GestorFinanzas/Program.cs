using GrandesLigas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using GrandesLigas.Models;

var builder = WebApplication.CreateBuilder(args);

// Cargar configuraci�n desde appsettings.Development.json
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

// Agregar el contexto de la base de datos
builder.Services.AddDbContext<GrandesLigasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestionFinanzasContext")));

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<EmailService>();

// Configurar `HttpClient`
builder.Services.AddHttpClient("ApiUsuario", client =>
{
    client.BaseAddress = new Uri("https://api.tuservicio.com/usuarios");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddAuthentication("MyCookieAuth") // Esquema de autenticaci�n
            .AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyAuthCookie"; // Nombre de la cookie
                options.LoginPath = "/Account/Login"; // Ruta de login
                options.LogoutPath = "/Account/Logout"; // Ruta de logout
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n
                options.SlidingExpiration = true; // Renovar la cookie si el usuario est� activo
            });

builder.Services.AddControllersWithViews();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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