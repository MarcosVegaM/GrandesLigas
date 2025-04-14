using GrandesLigas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrandesLigas.Controllers
{
    public class LigaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44395/api/Liga"; // Cambia según tu configuración

        public LigaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Liga
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var ligas = JsonSerializer.Deserialize<List<Liga>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(ligas);
        }

        // GET: Liga/Crear
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Liga liga)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(liga);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al crear la liga.";
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "Liga creada exitosamente.";
                return RedirectToAction("Index");
            }

            return View(liga);
        }

        // GET: Liga/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var liga = JsonSerializer.Deserialize<Liga>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(liga);
        }

        // POST: Liga/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Liga liga)
        {
            if (id != liga.Id) return NotFound();
          
            if (ModelState.IsValid)
            {
                // Obtener el ingreso actual desde la API
                var responseActual = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                if (!responseActual.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al obtener los datos del ingreso.";
                    return View(liga);
                }

                var jsonActual = await responseActual.Content.ReadAsStringAsync();
                var ligaActual = JsonSerializer.Deserialize<Liga>(jsonActual, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (ligaActual == null) return NotFound();

                // Serializar el objeto actualizado
                var ligaJson = JsonSerializer.Serialize(liga);
                var content = new StringContent(ligaJson, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT al API
                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Error al actualizar el ingreso.";
                    return View(liga);
                }

                TempData["SuccessMessage"] = "El ingreso se actualizó correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(liga);
        }


        // GET: Liga/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var liga = JsonSerializer.Deserialize<Liga>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(liga);
        }

        // POST: Liga/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Liga eliminada exitosamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar liga";
                return RedirectToAction("Index");
            }
        
        }
    }
}

