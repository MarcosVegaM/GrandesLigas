using Microsoft.AspNetCore.Mvc;
using GrandesLigas.Models;
using GrandesLigas.Services;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace GrandesLigas.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmailService _emailService;
        private readonly GrandesLigasContext _context;
        


        // GET: Account/RestablecerContraseña
        public IActionResult RestablecerContraseña()
        {
            return View();
        }

        // POST: Account/RestablecerContraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestablecerContraseña(RestablecerContraseñaModel model)
        {
            if (ModelState.IsValid)
            {
                // Busca al usuario por su correo electrónico
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == model.Email);
                if (usuario != null)
                {
                    // Genera un token único (GUID)
                    var token = Guid.NewGuid().ToString();

                    // Guarda el token en la base de datos
                    usuario.TokenRestablecimiento = token;
                    _context.SaveChanges();

                    // Genera el enlace de restablecimiento
                    var resetLink = Url.Action("ConfirmarRestablecimiento", "Account", new { token }, Request.Scheme);

                    // Envía el correo electrónico
                    var subject = "Restablecer Contraseña";
                    var body = $"Haz clic en el siguiente enlace para restablecer tu contraseña: <a href='{resetLink}'>Restablecer Contraseña</a>";
                    _emailService.SendEmail(model.Email, subject, body);

                    TempData["SuccessMessage"] = "Se ha enviado un correo con instrucciones para restablecer tu contraseña.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se encontró un usuario con ese correo electrónico.";
                }
            }

            return View(model);
        }

        // GET: Account/ConfirmarRestablecimiento
        public IActionResult ConfirmarRestablecimiento(string token)
        {
            // Aquí puedes validar el token y mostrar un formulario para ingresar una nueva contraseña
            ViewBag.Token = token;
            return View();
        }
        // Método para hashear la contraseña
        private string HashPassword(string password)
        {
           
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        [HttpPost]
        public IActionResult ConfirmarRestablecimiento(string token, string nuevaContraseña)
        {
            // 1. Busca al usuario por el token
            var usuario = _context.Usuarios.FirstOrDefault(u => u.TokenRestablecimiento == token);
            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Token inválido o usuario no encontrado.";
                return RedirectToAction("Index", "Home");
            }

            // 2. Hashea la nueva contraseña
            var hashedPassword = HashPassword(nuevaContraseña);

            // 3. Actualiza la contraseña y limpia el token
            usuario.Contraseña = hashedPassword;
            usuario.TokenRestablecimiento = null; // Limpia el token después de usarlo
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Tu contraseña ha sido restablecida exitosamente.";
            return RedirectToAction("Index", "Home");
        }

        public AccountController(GrandesLigasContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            
            //ClaimsIdentity identity = new ClaimsIdentity();
            if (ModelState.IsValid)
            {
                // Busca al usuario por su correo electrónico
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == usuario.Email);

                if (user != null)
                {
                    // Verifica la contraseña usando BCrypt
                    if (BCrypt.Net.BCrypt.Verify(usuario.Contraseña, user.Contraseña))
                    {
                         // Mensaje de éxito*/
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim("UserId", user.Id.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(
                            "MyCookieAuth",
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties
                            {
                                IsPersistent = true,  // Mantiene la sesión activa después de cerrar el navegador
                                ExpiresUtc = DateTime.UtcNow.AddDays(7) // Expira en 7 días
                            }
                         );

                        TempData["SuccessMessage"] = "Inicio de sesión exitoso.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Mensaje de error
                        ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrectos.");
                    }
                }
                else
                {
                    // Mensaje de error
                    ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrectos.");
                }
            }

            return View();
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión
            Response.Cookies.Delete("MyCookieAuth");
            // Invalidar la autenticación en el servidor
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
            await HttpContext.SignOutAsync("MyCookieAuth");

            //await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Account");
        }
    }
}

