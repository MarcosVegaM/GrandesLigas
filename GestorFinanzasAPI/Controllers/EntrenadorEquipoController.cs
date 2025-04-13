using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenadorEquipoController : ControllerBase
    {
        private readonly IEntrenadorEquipoService _entrenadorEquipoService;

        public EntrenadorEquipoController(IEntrenadorEquipoService entrenadorEquipoService)
        {
            _entrenadorEquipoService = entrenadorEquipoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntrenadoresEquipos()
        {
            var relaciones = await _entrenadorEquipoService.ObtenerEntrenadoresEquipos();
            return Ok(relaciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntrenadorEquipo(int id)
        {
            var relacion = await _entrenadorEquipoService.ObtenerEntrenadorEquipoPorId(id);
            if (relacion == null)
                return NotFound();
            return Ok(relacion);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntrenadorEquipo([FromBody] EntrenadorEquipo entrenadorEquipo)
        {
            if (entrenadorEquipo == null)
                return BadRequest();

            // Validar si ya existe la relación
            var existe = await _entrenadorEquipoService.ExisteRelacion(entrenadorEquipo.EntrenadorId, entrenadorEquipo.EquipoId);
            if (existe)
                return Conflict("Esta relación entrenador-equipo ya existe");

            await _entrenadorEquipoService.CrearEntrenadorEquipo(entrenadorEquipo);
            return CreatedAtAction(nameof(GetEntrenadorEquipo), new { id = entrenadorEquipo.EntrenadorEquipoId }, entrenadorEquipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntrenadorEquipo(int id, [FromBody] EntrenadorEquipo entrenadorEquipo)
        {
            if (entrenadorEquipo == null || id != entrenadorEquipo.EntrenadorEquipoId)
                return BadRequest();

            var relacionExistente = await _entrenadorEquipoService.ObtenerEntrenadorEquipoPorId(id);
            if (relacionExistente == null)
                return NotFound();

            await _entrenadorEquipoService.ActualizarEntrenadorEquipo(entrenadorEquipo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrenadorEquipo(int id)
        {
            var relacion = await _entrenadorEquipoService.ObtenerEntrenadorEquipoPorId(id);
            if (relacion == null)
                return NotFound();

            await _entrenadorEquipoService.EliminarEntrenadorEquipo(id);
            return NoContent();
        }

        // Endpoint adicional para buscar por entrenador
        [HttpGet("por-entrenador/{entrenadorId}")]
        public async Task<IActionResult> GetEquiposDeEntrenador(int entrenadorId)
        {
            var relaciones = await _entrenadorEquipoService.ObtenerEquiposIdsPorEntrenador(entrenadorId);
            return Ok(relaciones);
        }

        // Endpoint adicional para buscar por equipo
        [HttpGet("por-equipo/{equipoId}")]
        public async Task<IActionResult> GetEntrenadoresDeEquipo(int equipoId)
        {
            var relaciones = await _entrenadorEquipoService.ObtenerEntrenadoresIdsPorEquipo(equipoId);
            return Ok(relaciones);
        }
    }
}
