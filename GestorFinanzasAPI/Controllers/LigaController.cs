using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigaController : ControllerBase
    {
        private readonly ILigaService _ligaService;

        public LigaController(ILigaService ligaService)
        {
            _ligaService = ligaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLigas()
        {
            var ligas = await _ligaService.ObtenerLigas();
            return Ok(ligas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLiga(int id)
        {
            var liga = await _ligaService.ObtenerLigaPorId(id);
            if (liga == null)
                return NotFound();
            return Ok(liga);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLiga([FromBody] Liga liga)
        {
            if (liga == null)
                return BadRequest();

            await _ligaService.CrearLiga(liga);
            return CreatedAtAction(nameof(GetLiga), new { id = liga.Id }, liga);
        }

        // Este método PUT se encarga de actualizar la Liga
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLiga(int id, [FromBody] Liga liga)
        {
            if (liga == null || id != liga.Id)
                return BadRequest();

            var ligaExistente = await _ligaService.ObtenerLigaPorId(id);
            if (ligaExistente == null)
                return NotFound();

            await _ligaService.ActualizarLiga(liga);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiga(int id)
        {
            var liga = await _ligaService.ObtenerLigaPorId(id);
            if (liga == null)
                return NotFound();

            await _ligaService.EliminarLiga(id);
            return NoContent();
        }
    }
}


