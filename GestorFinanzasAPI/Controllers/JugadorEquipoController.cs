using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorEquipoController : ControllerBase
    {
        private readonly IJugadorEquipoService _jugadorEquipoService;

        public JugadorEquipoController(IJugadorEquipoService jugadorEquipoService)
        {
            _jugadorEquipoService = jugadorEquipoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJugadoresEquipos()
        {
            var relaciones = await _jugadorEquipoService.ObtenerJugadoresEquipos();
            return Ok(relaciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJugadorEquipo(int id)
        {
            var relacion = await _jugadorEquipoService.ObtenerJugadorEquipoPorId(id);
            if (relacion == null)
                return NotFound();
            return Ok(relacion);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJugadorEquipo([FromBody] JugadorEquipo jugadorEquipo)
        {
            if (jugadorEquipo == null)
                return BadRequest();

            // Validar si ya existe la relación
            var existe = await _jugadorEquipoService.ExisteRelacion(jugadorEquipo.JugadorId, jugadorEquipo.EquipoId);
            if (existe)
                return Conflict("Esta relación jugador-equipo ya existe");

            await _jugadorEquipoService.CrearJugadorEquipo(jugadorEquipo);
            return CreatedAtAction(nameof(GetJugadorEquipo), new { id = jugadorEquipo.JugadorEquipoId }, jugadorEquipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJugadorEquipo(int id, [FromBody] JugadorEquipo jugadorEquipo)
        {
            if (jugadorEquipo == null || id != jugadorEquipo.JugadorEquipoId)
                return BadRequest();

            var relacionExistente = await _jugadorEquipoService.ObtenerJugadorEquipoPorId(id);
            if (relacionExistente == null)
                return NotFound();

            await _jugadorEquipoService.ActualizarJugadorEquipo(jugadorEquipo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJugadorEquipo(int id)
        {
            var relacion = await _jugadorEquipoService.ObtenerJugadorEquipoPorId(id);
            if (relacion == null)
                return NotFound();

            await _jugadorEquipoService.EliminarJugadorEquipo(id);
            return NoContent();
        }

        // Endpoint adicional para buscar por jugador
        [HttpGet("por-jugador/{jugadorId}")]
        public async Task<IActionResult> GetEquiposDeJugador(int jugadorId)
        {
            var relaciones = await _jugadorEquipoService.ObtenerJugadoresIdsPorEquipo(jugadorId);
            return Ok(relaciones);
        }

        // Endpoint adicional para buscar por equipo
        [HttpGet("por-equipo/{equipoId}")]
        public async Task<IActionResult> GetJugadoresDeEquipo(int equipoId)
        {
            var relaciones = await _jugadorEquipoService.ObtenerEquiposIdsPorJugador(equipoId);
            return Ok(relaciones);
        }
    }
}
