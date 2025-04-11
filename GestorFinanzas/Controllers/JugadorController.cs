using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrandesLigas.Controllers
{
    public class JugadorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Jugador"; // Ajusta la URL según tu API

        public JugadorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Jugador
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jugadores = JsonSerializer.Deserialize<List<Jugador>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(jugadores);
        }

        // GET: Jugador/Crear
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(jugador);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear el jugador.";
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "Jugador creado exitosamente.";
                return RedirectToAction("Index");
            }

            return View(jugador);
        }

        // GET: Jugador/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jugador = JsonSerializer.Deserialize<Jugador>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(jugador);
        }

        // POST: Jugador/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Jugador jugador)
        {
            if (id != jugador.JugadorId) return NotFound();

            if (ModelState.IsValid)
            {
                var responseActual = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                if (!responseActual.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al obtener los datos del jugador.";
                    return View(jugador);
                }

                var jsonActual = await responseActual.Content.ReadAsStringAsync();
                var jugadorActual = JsonSerializer.Deserialize<Jugador>(jsonActual, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (jugadorActual == null) return NotFound();

                var jugadorJson = JsonSerializer.Serialize(jugador);
                var content = new StringContent(jugadorJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el jugador.";
                    return View(jugador);
                }

                TempData["SuccessMessage"] = "Jugador actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(jugador);
        }

        // GET: Jugador/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jugador = JsonSerializer.Deserialize<Jugador>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(jugador);
        }

        // POST: Jugador/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Jugador eliminado exitosamente.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
