using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrandesLigas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Usuario"; // Reemplazar con la URL del API

        public UsuarioController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(usuarios);
        }

        // GET: Usuario/Crear
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Hashear la contraseña antes de enviarla al API
                usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

                var jsonContent = JsonSerializer.Serialize(usuario);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear el usuario.";
                    return RedirectToAction("Index", "Usuario");
                }

                TempData["SuccessMessage"] = "Se ha creado el usuario de manera exitosa.";
                return RedirectToAction("Index", "Usuario");
            }

            return View(usuario);
        }


        // GET: Usuario/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var usuario = JsonSerializer.Deserialize<Usuario>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(usuario);
        }

        // POST: Usuario/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Liga liga)
        {
            if (id != liga.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Obtener la liga actual desde el API
                var responseActual = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                if (!responseActual.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al obtener los datos de la liga.";
                    return View(liga);
                }

                var jsonActual = await responseActual.Content.ReadAsStringAsync();
                var ligaActual = JsonSerializer.Deserialize<Liga>(jsonActual, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (ligaActual == null) return NotFound();

                // Aquí podrías hacer comparaciones si es necesario, por ejemplo:
                // if (liga.Nombre != ligaActual.Nombre) { ... }

                // Serializar la liga actualizada
                var ligaJson = JsonSerializer.Serialize(liga);
                var content = new StringContent(ligaJson, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT al API
                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar la liga.";
                    return View(liga);
                }

                TempData["SuccessMessage"] = "Liga modificada de manera exitosa.";
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, regresar a la vista con los errores
            return View(liga);
        }




        // GET: Usuario/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var usuario = JsonSerializer.Deserialize<Usuario>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(usuario);
        }

        // POST: Usuario/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Usuario eliminado de manera exitosa";
                return RedirectToAction("Index", "Usuario");
            }
            return NotFound();
        }
    }
}

