using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidoController : ControllerBase
    {
        private readonly IPartidoService _partidoService;

        public PartidoController(IPartidoService partidoService)
        {
            _partidoService = partidoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPartidos()
        {
            var partidos = await _partidoService.ObtenerPartidos();
            return Ok(partidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartido(int id)
        {
            var partido = await _partidoService.ObtenerPartidoPorId(id);
            if (partido == null)
                return NotFound();
            return Ok(partido);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartido([FromBody] Partido partido)
        {
            if (partido == null)
                return BadRequest();

            await _partidoService.CrearPartido(partido);
            return CreatedAtAction(nameof(GetPartido), new { id = partido.PartidoId }, partido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePartido(int id, [FromBody] Partido partido)
        {
            if (partido == null || id != partido.PartidoId)
                return BadRequest();

            var partidoExistente = await _partidoService.ObtenerPartidoPorId(id);
            if (partidoExistente == null)
                return NotFound();

            await _partidoService.ActualizarPartido(partido);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            var partido = await _partidoService.ObtenerPartidoPorId(id);
            if (partido == null)
                return NotFound();

            await _partidoService.EliminarPartido(id);
            return NoContent();
        }
        [HttpGet("porliga/{ligaId}")]
        public async Task<IActionResult> GetPartidosPorLiga(int ligaId)
        {
            var partidos = await _partidoService.ObtenerPartidosPorLiga(ligaId);
            return Ok(partidos);
        }
    }
}
