// Controllers/ClasificacionController.cs
using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrandesLigas.Controllers
{
    public class ClasificacionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Clasificacion";
        private readonly string _apiLigasUrl = "https://localhost:44395/api/Liga";
        private readonly string _apiEquiposUrl = "https://localhost:44395/api/Equipo";

        public ClasificacionController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Clasificacion
        public async Task<IActionResult> Index()
        {
            // Procesar equipos
            var equiposTask = _httpClient.GetAsync(_apiEquiposUrl);
            var equipos = new List<Equipo>();
            if (equiposTask.Result.IsSuccessStatusCode)
            {
                var equiposJson = await equiposTask.Result.Content.ReadAsStringAsync();
                equipos = JsonSerializer.Deserialize<List<Equipo>>(
                    equiposJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            ViewBag.Equipos = equipos ?? new List<Equipo>();

            try
            {
                // Cargar clasificaciones y ligas en paralelo
                var clasificacionesTask = _httpClient.GetAsync(_apiUrl);
                var ligasTask = _httpClient.GetAsync(_apiLigasUrl);

                await Task.WhenAll(clasificacionesTask, ligasTask);

                // Procesar clasificaciones
                if (!clasificacionesTask.Result.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar las clasificaciones";
                    return View(new List<Clasificacion>());
                }

                var clasificacionesJson = await clasificacionesTask.Result.Content.ReadAsStringAsync();
                var clasificaciones = JsonSerializer.Deserialize<List<Clasificacion>>(
                    clasificacionesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Procesar ligas
                var ligas = new List<Liga>();
                if (ligasTask.Result.IsSuccessStatusCode)
                {
                    var ligasJson = await ligasTask.Result.Content.ReadAsStringAsync();
                    ligas = JsonSerializer.Deserialize<List<Liga>>(
                        ligasJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                ViewBag.Ligas = ligas ?? new List<Liga>();
                return View(clasificaciones);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<Clasificacion>());
            }
        }

        // GET: Clasificacion/PorLiga/5
        public async Task<IActionResult> PorLiga(int id)
        {
            try
            {
                // Obtener clasificación por liga
                var response = await _httpClient.GetAsync($"{_apiUrl}/porliga/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar la clasificación de la liga";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var clasificaciones = JsonSerializer.Deserialize<List<Clasificacion>>(
                    jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener información de la liga
                var ligaResponse = await _httpClient.GetAsync($"{_apiLigasUrl}/{id}");
                var liga = new Liga();
                if (ligaResponse.IsSuccessStatusCode)
                {
                    var ligaJson = await ligaResponse.Content.ReadAsStringAsync();
                    liga = JsonSerializer.Deserialize<Liga>(
                        ligaJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                // Obtener lista de equipos para mostrar nombres completos
                var equiposResponse = await _httpClient.GetAsync(_apiEquiposUrl);
                var equipos = new List<Equipo>();
                if (equiposResponse.IsSuccessStatusCode)
                {
                    var equiposJson = await equiposResponse.Content.ReadAsStringAsync();
                    equipos = JsonSerializer.Deserialize<List<Equipo>>(
                        equiposJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                ViewBag.Liga = liga;
                ViewBag.Equipos = equipos;
                return View(clasificaciones);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        
        // Método auxiliar para cargar ligas en el ViewBag como SelectListItem
        private async Task CargarLigasEnViewBag()
        {
            var response = await _httpClient.GetAsync(_apiLigasUrl);
            var ligas = new List<SelectListItem>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var listaLigas = JsonSerializer.Deserialize<List<Liga>>(
                    jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ligas = listaLigas.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Nombre
                }).ToList();
            }

            ViewBag.Ligas = ligas;
        }
    }
}
