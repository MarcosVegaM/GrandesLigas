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
    public class JugadorEquipoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/JugadorEquipo";
        private readonly string _apiJugadoresUrl = "https://localhost:44395/api/Jugador";
        private readonly string _apiEquiposUrl = "https://localhost:44395/api/Equipo";

        public JugadorEquipoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: JugadorEquipo
        public async Task<IActionResult> Index()
        {
            try
            {
                // Cargar relaciones, jugadores y equipos en paralelo
                var relacionesTask = _httpClient.GetAsync(_apiUrl);
                var jugadoresTask = _httpClient.GetAsync(_apiJugadoresUrl);
                var equiposTask = _httpClient.GetAsync(_apiEquiposUrl);

                await Task.WhenAll(relacionesTask, jugadoresTask, equiposTask);

                // Procesar relaciones
                if (!relacionesTask.Result.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al cargar las relaciones";
                    return View(new List<JugadorEquipo>());
                }

                var relacionesJson = await relacionesTask.Result.Content.ReadAsStringAsync();
                var relaciones = JsonSerializer.Deserialize<List<JugadorEquipo>>(
                    relacionesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Procesar jugadores y equipos
                var jugadores = new List<Jugador>();
                if (jugadoresTask.Result.IsSuccessStatusCode)
                {
                    var jugadoresJson = await jugadoresTask.Result.Content.ReadAsStringAsync();
                    jugadores = JsonSerializer.Deserialize<List<Jugador>>(
                        jugadoresJson,
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

                ViewBag.Jugadores = jugadores ?? new List<Jugador>();
                ViewBag.Equipos = equipos ?? new List<Equipo>();

                return View(relaciones);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<JugadorEquipo>());
            }
        }

        // GET: JugadorEquipo/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarJugadoresYEquiposEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(JugadorEquipo jugadorEquipo)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(jugadorEquipo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el jugador.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Jugador actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarJugadoresYEquiposEnViewBag();
            return View(jugadorEquipo);
        }

        // GET: JugadorEquipo/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var relacion = JsonSerializer.Deserialize<JugadorEquipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await CargarJugadoresYEquiposEnViewBag();
            return View(relacion);
        }

        // POST: JugadorEquipo/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, JugadorEquipo jugadorEquipo)
        {
            if (id != jugadorEquipo.JugadorEquipoId) return NotFound();

            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(jugadorEquipo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar la relación jugador-equipo.";
                    await CargarJugadoresYEquiposEnViewBag();
                    return View(jugadorEquipo);
                }

                TempData["SuccessMessage"] = "Relación jugador-equipo actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarJugadoresYEquiposEnViewBag();
            return View(jugadorEquipo);
        }

        // GET: JugadorEquipo/Eliminar/5
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
                var relacion = JsonSerializer.Deserialize<JugadorEquipo>(jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Obtener datos del jugador
                var jugadorResponse = await _httpClient.GetAsync($"{_apiJugadoresUrl}/{relacion.JugadorId}");
                if (!jugadorResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener la información del jugador";
                    return RedirectToAction(nameof(Index));
                }
                var jugadorJson = await jugadorResponse.Content.ReadAsStringAsync();
                var jugador = JsonSerializer.Deserialize<Jugador>(jugadorJson,
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

                ViewBag.Jugador = jugador;
                ViewBag.Equipo = equipo;

                return View(relacion);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: JugadorEquipo/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "La relación jugador-equipo fue eliminada exitosamente";
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

        // GET: JugadorEquipo/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var relacion = JsonSerializer.Deserialize<JugadorEquipo>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(relacion);
        }

        // Método auxiliar para cargar jugadores y equipos en el ViewBag
        private async Task CargarJugadoresYEquiposEnViewBag()
        {
            // Cargar jugadores
            var jugadoresResponse = await _httpClient.GetAsync(_apiJugadoresUrl);
            var jugadores = new List<SelectListItem>();

            if (jugadoresResponse.IsSuccessStatusCode)
            {
                var jsonString = await jugadoresResponse.Content.ReadAsStringAsync();
                var listaJugadores = JsonSerializer.Deserialize<List<Jugador>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                jugadores = listaJugadores.Select(j => new SelectListItem
                {
                    Value = j.JugadorId.ToString(),
                    Text = $"{j.Nombre} ({j.Posicion})"
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

            ViewBag.Jugadores = jugadores;
            ViewBag.Equipos = equipos;
        }
    }
}
