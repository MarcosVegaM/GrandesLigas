using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GrandesLigasAPI.Controllers
{
    public class PartidoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Partido";
        private readonly string _apiLigasUrl = "https://localhost:44395/api/Liga";
        private readonly string _apiEquiposUrl = "https://localhost:44395/api/Equipo";

        public PartidoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Partido
        public async Task<IActionResult> Index()
        {
            try
            {
                // Cargar partidos, ligas y equipos en paralelo
                var partidosTask = _httpClient.GetAsync(_apiUrl);
                var ligasTask = _httpClient.GetAsync(_apiLigasUrl);
                var equiposTask = _httpClient.GetAsync(_apiEquiposUrl);

                await Task.WhenAll(partidosTask, ligasTask, equiposTask);

                // Procesar partidos
                if (!partidosTask.Result.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar los partidos";
                    return View(new List<Partido>());
                }

                var partidosJson = await partidosTask.Result.Content.ReadAsStringAsync();
                var partidos = JsonSerializer.Deserialize<List<Partido>>(
                    partidosJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Procesar ligas y equipos
                var ligas = new List<Liga>();
                if (ligasTask.Result.IsSuccessStatusCode)
                {
                    var ligasJson = await ligasTask.Result.Content.ReadAsStringAsync();
                    ligas = JsonSerializer.Deserialize<List<Liga>>(
                        ligasJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var equipos = new List<Equipo>();
                if (equiposTask.Result.IsSuccessStatusCode)
                {
                    var equiposJson = await equiposTask.Result.Content.ReadAsStringAsync();
                    equipos = JsonSerializer.Deserialize<List<Equipo>>(
                        equiposJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                ViewBag.Ligas = ligas ?? new List<Liga>();
                ViewBag.Equipos = equipos ?? new List<Equipo>();

                return View(partidos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<Partido>());
            }
        }

        // GET: Partido/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarLigasYEquiposEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Partido partido)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(partido);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear el partido.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Partido creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarLigasYEquiposEnViewBag();
            return View(partido);
        }

        // GET: Partido/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var partido = JsonSerializer.Deserialize<Partido>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await CargarLigasYEquiposEnViewBag();
            return View(partido);
        }

        // POST: Partido/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Partido partido)
        {
            if (id != partido.PartidoId) return NotFound();

            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(partido);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el partido.";
                    await CargarLigasYEquiposEnViewBag();
                    return View(partido);
                }

                TempData["SuccessMessage"] = "Partido actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarLigasYEquiposEnViewBag();
            return View(partido);
        }

        // GET: Partido/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                // Obtener el partido
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se encontró el partido especificado";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var partido = JsonSerializer.Deserialize<Partido>(jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener datos de la liga
                var ligaResponse = await _httpClient.GetAsync($"{_apiLigasUrl}/{partido.LigaId}");
                if (!ligaResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la información de la liga";
                    return RedirectToAction(nameof(Index));
                }
                var ligaJson = await ligaResponse.Content.ReadAsStringAsync();
                var liga = JsonSerializer.Deserialize<Liga>(ligaJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener datos de los equipos
                var equipoLocalResponse = await _httpClient.GetAsync($"{_apiEquiposUrl}/{partido.EquipoLocalId}");
                var equipoVisitanteResponse = await _httpClient.GetAsync($"{_apiEquiposUrl}/{partido.EquipoVisitanteId}");

                var equipoLocal = new Equipo();
                if (equipoLocalResponse.IsSuccessStatusCode)
                {
                    var equipoLocalJson = await equipoLocalResponse.Content.ReadAsStringAsync();
                    equipoLocal = JsonSerializer.Deserialize<Equipo>(equipoLocalJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var equipoVisitante = new Equipo();
                if (equipoVisitanteResponse.IsSuccessStatusCode)
                {
                    var equipoVisitanteJson = await equipoVisitanteResponse.Content.ReadAsStringAsync();
                    equipoVisitante = JsonSerializer.Deserialize<Equipo>(equipoVisitanteJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                ViewBag.Liga = liga;
                ViewBag.EquipoLocal = equipoLocal;
                ViewBag.EquipoVisitante = equipoVisitante;

                return View(partido);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Partido/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "El partido fue eliminado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el partido";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Partido/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var partido = JsonSerializer.Deserialize<Partido>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(partido);
        }

        // Método auxiliar para cargar ligas y equipos en el ViewBag como SelectListItem
        private async Task CargarLigasYEquiposEnViewBag()
        {
            // Cargar ligas
            var ligasResponse = await _httpClient.GetAsync(_apiLigasUrl);
            var ligas = new List<SelectListItem>();

            if (ligasResponse.IsSuccessStatusCode)
            {
                var jsonString = await ligasResponse.Content.ReadAsStringAsync();
                var listaLigas = JsonSerializer.Deserialize<List<Liga>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ligas = listaLigas.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Nombre
                }).ToList();
            }

            // Cargar equipos
            var equiposResponse = await _httpClient.GetAsync(_apiEquiposUrl);
            var equipos = new List<SelectListItem>();

            if (equiposResponse.IsSuccessStatusCode)
            {
                var jsonString = await equiposResponse.Content.ReadAsStringAsync();
                var listaEquipos = JsonSerializer.Deserialize<List<Equipo>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                equipos = listaEquipos.Select(e => new SelectListItem
                {
                    Value = e.EquipoId.ToString(),
                    Text = $"{e.Nombre} ({e.Ciudad})"
                }).ToList();
            }

            ViewBag.Ligas = ligas;
            ViewBag.Equipos = equipos;
        }
    }
}
