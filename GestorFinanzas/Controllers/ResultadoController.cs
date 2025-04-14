// Controllers/ResultadoController.cs
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
    public class ResultadoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Resultado";
        private readonly string _apiPartidosUrl = "https://localhost:44395/api/Partido";
        private readonly string _apiClasificacionesUrl = "https://localhost:44395/api/Clasificacion";
        private readonly string _apiEquiposUrl = "https://localhost:44395/api/Equipo";

        public ResultadoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Resultado
        public async Task<IActionResult> Index()
        {
            try
            {
                // Cargar resultados y partidos en paralelo
                var resultadosTask = _httpClient.GetAsync(_apiUrl);
                var partidosTask = _httpClient.GetAsync(_apiPartidosUrl);
                var equiposTask = _httpClient.GetAsync(_apiEquiposUrl);

                await Task.WhenAll(resultadosTask, partidosTask);

                // Procesar resultados
                if (!resultadosTask.Result.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar los resultados";
                    return View(new List<Resultado>());
                }

                var resultadosJson = await resultadosTask.Result.Content.ReadAsStringAsync();
                var resultados = JsonSerializer.Deserialize<List<Resultado>>(
                    resultadosJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Procesar partidos
                var partidos = new List<Partido>();
                if (partidosTask.Result.IsSuccessStatusCode)
                {
                    var partidosJson = await partidosTask.Result.Content.ReadAsStringAsync();
                    partidos = JsonSerializer.Deserialize<List<Partido>>(
                        partidosJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                ViewBag.Partidos = partidos ?? new List<Partido>();


                // Procesar equipos
                var equipos = new List<Equipo>();
                if (equiposTask.Result.IsSuccessStatusCode)
                {
                    var equiposJson = await equiposTask.Result.Content.ReadAsStringAsync();
                    equipos = JsonSerializer.Deserialize<List<Equipo>>(
                        equiposJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                ViewBag.Equipos = equipos ?? new List<Equipo>();

                return View(resultados);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<Resultado>());
            }
        }

        // GET: Resultado/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarPartidosEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Resultado resultado)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(resultado);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear el resultado.";
                    await CargarPartidosEnViewBag();
                    return View(resultado);
                }

                TempData["SuccessMessage"] = "Resultado creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarPartidosEnViewBag();
            return View(resultado);
        }

        // GET: Resultado/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultado = JsonSerializer.Deserialize<Resultado>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await CargarPartidosEnViewBag(resultado.PartidoId);
            return View(resultado);
        }

        // POST: Resultado/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Resultado resultado)
        {
            if (id != resultado.ResultadoId) return NotFound();

            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(resultado);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el resultado.";
                    await CargarPartidosEnViewBag();
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Resultado actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarPartidosEnViewBag();
            return View(resultado);
        }

        // GET: Resultado/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                // Obtener el resultado
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se encontró el resultado especificado";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<Resultado>(jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener datos del partido
                var partidoResponse = await _httpClient.GetAsync($"{_apiPartidosUrl}/{resultado.PartidoId}");
                if (!partidoResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la información del partido";
                    return RedirectToAction(nameof(Index));
                }
                var partidoJson = await partidoResponse.Content.ReadAsStringAsync();
                var partido = JsonSerializer.Deserialize<Partido>(partidoJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.Partido = partido;
                await CargarPartidosEnViewBag(id);
                return View(resultado);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Resultado/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "El resultado fue eliminado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el resultado";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        
        // Método auxiliar para cargar partidos en el ViewBag como SelectListItem
        // Método auxiliar para cargar partidos y equipos en el ViewBag
        private async Task CargarPartidosEnViewBag(int? partidoId = null)
        {
            // Cargar partidos y equipos en paralelo
            var partidosTask = _httpClient.GetAsync(_apiPartidosUrl);
            var equiposTask = _httpClient.GetAsync(_apiEquiposUrl);

            await Task.WhenAll(partidosTask, equiposTask);

            // Procesar equipos
            var equipos = new List<Equipo>();
            if (equiposTask.Result.IsSuccessStatusCode)
            {
                var equiposJson = await equiposTask.Result.Content.ReadAsStringAsync();
                equipos = JsonSerializer.Deserialize<List<Equipo>>(
                    equiposJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            ViewBag.Equipos = equipos ?? new List<Equipo>();

            // Procesar partidos
            var partidos = new List<SelectListItem>();
            Partido partidoSeleccionado = null;

            if (partidosTask.Result.IsSuccessStatusCode)
            {
                var jsonString = await partidosTask.Result.Content.ReadAsStringAsync();
                var listaPartidos = JsonSerializer.Deserialize<List<Partido>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Si estamos editando, obtener el partido específico
                if (partidoId.HasValue)
                {
                    partidoSeleccionado = listaPartidos.FirstOrDefault(p => p.PartidoId == partidoId.Value);
                }

                partidos = listaPartidos.Select(p =>
                {
                    var equipoLocal = equipos?.FirstOrDefault(e => e.EquipoId == p.EquipoLocalId);
                    var equipoVisitante = equipos?.FirstOrDefault(e => e.EquipoId == p.EquipoVisitanteId);

                    return new SelectListItem
                    {
                        Value = p.PartidoId.ToString(),
                        Text = $"Partido {p.PartidoId}: {equipoLocal?.Nombre ?? "Local"} vs {equipoVisitante?.Nombre ?? "Visitante"}",
                        Selected = partidoSeleccionado?.PartidoId == p.PartidoId
                    };
                }).ToList();

                // Guardar también la lista completa para acceso directo
                ViewBag.PartidosLista = listaPartidos;
            }

            ViewBag.Partidos = partidos;

            // Si estamos editando, devolver el partido seleccionado
            if (partidoId.HasValue)
            {
                ViewBag.PartidoSeleccionado = partidoSeleccionado;
            }
        }
    }
}
