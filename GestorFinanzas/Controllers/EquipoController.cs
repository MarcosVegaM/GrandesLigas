using GrandesLigasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrandesLigasAPI.Controllers
{
    public class EquipoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Equipo"; // URL API Equipo
        private readonly string _apiLigasUrl = "https://localhost:44395/api/Liga"; // URL API Liga

        public EquipoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Equipo
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var equipos = JsonSerializer.Deserialize<List<Equipo>>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(equipos);
        }

        // GET: Equipo/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarLigasEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(equipo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear el equipo.";
                    await CargarLigasEnViewBag();
                    return View(equipo);
                }

                TempData["SuccessMessage"] = "Equipo creado exitosamente.";
                return RedirectToAction("Index");
            }

            await CargarLigasEnViewBag();
            return View(equipo);
        }

        // GET: Equipo/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var equipo = JsonSerializer.Deserialize<Equipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await CargarLigasEnViewBag();
            return View(equipo);
        }

        // POST: Equipo/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Equipo equipo)
        {
            if (id != equipo.EquipoId) return NotFound();

            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(equipo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el equipo.";
                    await CargarLigasEnViewBag();
                    return View(equipo);
                }

                TempData["SuccessMessage"] = "Equipo actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarLigasEnViewBag();
            return View(equipo);
        }

        // GET: Equipo/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var equipo = JsonSerializer.Deserialize<Equipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(equipo);
        }

        // POST: Equipo/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Equipo eliminado exitosamente.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: Equipo/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var equipo = JsonSerializer.Deserialize<Equipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(equipo);
        }

        // Método auxiliar para cargar las ligas en el ViewBag
        private async Task CargarLigasEnViewBag()
        {
            List<SelectListItem> ligas = new List<SelectListItem>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44395/api/");

                // Obtener las ligas desde la API
                HttpResponseMessage response = client.GetAsync("Liga").GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var listaLigas = JsonSerializer.Deserialize<List<Liga>>(jsonString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Convertir a SelectListItem
                    ligas = listaLigas.Select(l =>
                        new SelectListItem
                        {
                            Value = l.Id.ToString(),
                            Text = l.Nombre
                        }
                    ).ToList();
                }
                else
                {
                    // Manejar error si la API no responde
                    TempData["ErrorMessage"] = "No se pudieron cargar las ligas";
                }
            }

            ViewData["Ligas"] = ligas;
        }
    }
}
