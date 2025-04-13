using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GrandesLigas.Controllers
{
    public class CalendarioController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:44395/api/";

        public CalendarioController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int? ligaId)
        {
            try
            {
                // Cargar todas las ligas
                var ligasResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Liga");
                if (!ligasResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar las ligas";
                    return View(new List<FechaPartidos>());
                }

                var ligasJson = await ligasResponse.Content.ReadAsStringAsync();
                var ligas = JsonSerializer.Deserialize<List<Liga>>(
                    ligasJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new List<Liga>();

                // Cargar equipos
                var equiposResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Equipo");
                if (!equiposResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar los equipos";
                    return View(new List<FechaPartidos>());
                }

                var equiposJson = await equiposResponse.Content.ReadAsStringAsync();
                var equipos = JsonSerializer.Deserialize<List<Equipo>>(
                    equiposJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new List<Equipo>();

                // Cargar partidos (filtrados o todos)
                var partidosResponse = await _httpClient.GetAsync(
                    ligaId.HasValue ? $"{_apiBaseUrl}Partido/porliga/{ligaId.Value}" : $"{_apiBaseUrl}Partido");

                if (!partidosResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar los partidos";
                    return View(new List<FechaPartidos>());
                }

                var partidosJson = await partidosResponse.Content.ReadAsStringAsync();
                var partidos = JsonSerializer.Deserialize<List<Partido>>(
                    partidosJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new List<Partido>();

                // Preparar datos para la vista
                ViewBag.Ligas = new SelectList(ligas, "Id", "Nombre", ligaId);
                ViewBag.Equipos = equipos.ToDictionary(e => e.EquipoId, e => e.Nombre);
                ViewBag.LigaSeleccionadaId = ligaId;

                // Agrupar partidos por fecha
                var calendario = partidos
                    .OrderBy(p => p.Fecha)
                    .GroupBy(p => p.Fecha.Date)
                    .Select(g => new FechaPartidos
                    {
                        Fecha = g.Key,
                        Partidos = g.ToList()
                    })
                    .OrderBy(f => f.Fecha)
                    .ToList();

                return View(calendario);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
                return View(new List<FechaPartidos>());
            }
        }

        public class FechaPartidos
        {
            public DateTime Fecha { get; set; }
            public List<Partido> Partidos { get; set; } = new List<Partido>();
        }
    }
}