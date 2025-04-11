using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrandesLigas.Controllers
{
    public class EntrenadorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Entrenador"; // Ajusta la URL según tu API

        public EntrenadorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Entrenador
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var entrenadores = JsonSerializer.Deserialize<List<Entrenador>>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(entrenadores);
        }

        // GET: Entrenador/Crear
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Entrenador entrenador)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(entrenador);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear el entrenador.";
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "Entrenador creado exitosamente.";
                return RedirectToAction("Index");
            }

            return View(entrenador);
        }

        // GET: Entrenador/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var entrenador = JsonSerializer.Deserialize<Entrenador>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(entrenador);
        }

        // POST: Entrenador/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Entrenador entrenador)
        {
            if (id != entrenador.EntrenadorId) return NotFound();

            if (ModelState.IsValid)
            {
                var responseActual = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                if (!responseActual.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al obtener los datos del entrenador.";
                    return View(entrenador);
                }

                var jsonActual = await responseActual.Content.ReadAsStringAsync();
                var entrenadorActual = JsonSerializer.Deserialize<Entrenador>(jsonActual,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (entrenadorActual == null) return NotFound();

                var entrenadorJson = JsonSerializer.Serialize(entrenador);
                var content = new StringContent(entrenadorJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el entrenador.";
                    return View(entrenador);
                }

                TempData["SuccessMessage"] = "Entrenador actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(entrenador);
        }

        // GET: Entrenador/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var entrenador = JsonSerializer.Deserialize<Entrenador>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(entrenador);
        }

        // POST: Entrenador/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Entrenador eliminado exitosamente.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: Entrenador/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var entrenador = JsonSerializer.Deserialize<Entrenador>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(entrenador);
        }
    }
}