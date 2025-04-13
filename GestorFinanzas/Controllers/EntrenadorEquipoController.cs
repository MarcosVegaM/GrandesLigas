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
    public class EntrenadorEquipoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/EntrenadorEquipo";
        private readonly string _apiEntrenadoresUrl = "https://localhost:44395/api/Entrenador";
        private readonly string _apiEquiposUrl = "https://localhost:44395/api/Equipo";

        public EntrenadorEquipoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: EntrenadorEquipo
        public async Task<IActionResult> Index()
        {
            try
            {
                // Cargar relaciones, entrenadores y equipos en paralelo
                var relacionesTask = _httpClient.GetAsync(_apiUrl);
                var entrenadoresTask = _httpClient.GetAsync(_apiEntrenadoresUrl);
                var equiposTask = _httpClient.GetAsync(_apiEquiposUrl);

                await Task.WhenAll(relacionesTask, entrenadoresTask, equiposTask);

                // Procesar relaciones
                if (!relacionesTask.Result.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar las relaciones";
                    return View(new List<EntrenadorEquipo>());
                }

                var relacionesJson = await relacionesTask.Result.Content.ReadAsStringAsync();
                var relaciones = JsonSerializer.Deserialize<List<EntrenadorEquipo>>(
                    relacionesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Procesar entrenadores y equipos
                var entrenadores = new List<Entrenador>();
                if (entrenadoresTask.Result.IsSuccessStatusCode)
                {
                    var entrenadoresJson = await entrenadoresTask.Result.Content.ReadAsStringAsync();
                    entrenadores = JsonSerializer.Deserialize<List<Entrenador>>(
                        entrenadoresJson,
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

                ViewBag.Entrenadores = entrenadores ?? new List<Entrenador>();
                ViewBag.Equipos = equipos ?? new List<Equipo>();

                return View(relaciones);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<EntrenadorEquipo>());
            }
        }

        // GET: EntrenadorEquipo/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarEntrenadoresYEquiposEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(EntrenadorEquipo entrenadorEquipo)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(entrenadorEquipo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear la relación entrenador-equipo.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Relación entrenador-equipo creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarEntrenadoresYEquiposEnViewBag();
            return View(entrenadorEquipo);
        }

        // GET: EntrenadorEquipo/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var relacion = JsonSerializer.Deserialize<EntrenadorEquipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await CargarEntrenadoresYEquiposEnViewBag();
            return View(relacion);
        }

        // POST: EntrenadorEquipo/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, EntrenadorEquipo entrenadorEquipo)
        {
            if (id != entrenadorEquipo.EntrenadorEquipoId) return NotFound();

            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(entrenadorEquipo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar la relación entrenador-equipo.";
                    await CargarEntrenadoresYEquiposEnViewBag();
                    return View(entrenadorEquipo);
                }

                TempData["SuccessMessage"] = "Relación entrenador-equipo actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarEntrenadoresYEquiposEnViewBag();
            return View(entrenadorEquipo);
        }

        // GET: EntrenadorEquipo/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                // Obtener la relación
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se encontró la relación especificada";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var relacion = JsonSerializer.Deserialize<EntrenadorEquipo>(jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener datos del entrenador
                var entrenadorResponse = await _httpClient.GetAsync($"{_apiEntrenadoresUrl}/{relacion.EntrenadorId}");
                if (!entrenadorResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la información del entrenador";
                    return RedirectToAction(nameof(Index));
                }
                var entrenadorJson = await entrenadorResponse.Content.ReadAsStringAsync();
                var entrenador = JsonSerializer.Deserialize<Entrenador>(entrenadorJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener datos del equipo
                var equipoResponse = await _httpClient.GetAsync($"{_apiEquiposUrl}/{relacion.EquipoId}");
                if (!equipoResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la información del equipo";
                    return RedirectToAction(nameof(Index));
                }
                var equipoJson = await equipoResponse.Content.ReadAsStringAsync();
                var equipo = JsonSerializer.Deserialize<Equipo>(equipoJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.Entrenador = entrenador;
                ViewBag.Equipo = equipo;

                return View(relacion);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EntrenadorEquipo/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "La relación entrenador-equipo fue eliminada exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar la relación";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EntrenadorEquipo/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var relacion = JsonSerializer.Deserialize<EntrenadorEquipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(relacion);
        }

        // Método auxiliar para cargar entrenadores y equipos en el ViewBag
        private async Task CargarEntrenadoresYEquiposEnViewBag()
        {
            // Cargar entrenadores
            var entrenadoresResponse = await _httpClient.GetAsync(_apiEntrenadoresUrl);
            var entrenadores = new List<SelectListItem>();

            if (entrenadoresResponse.IsSuccessStatusCode)
            {
                var jsonString = await entrenadoresResponse.Content.ReadAsStringAsync();
                var listaEntrenadores = JsonSerializer.Deserialize<List<Entrenador>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                entrenadores = listaEntrenadores.Select(e => new SelectListItem
                {
                    Value = e.EntrenadorId.ToString(),
                    Text = $"{e.Nombre} ({e.Especialidad})"
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

            ViewBag.Entrenadores = entrenadores;
            ViewBag.Equipos = equipos;
        }
    }
}
