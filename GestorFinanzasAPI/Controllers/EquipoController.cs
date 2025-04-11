using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly IEquipoService _equipoService;

        public EquipoController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEquipos()
        {
            var equipos = await _equipoService.ObtenerEquipos();
            return Ok(equipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipo(int id)
        {
            var equipo = await _equipoService.ObtenerEquipoPorId(id);
            if (equipo == null)
                return NotFound();
            return Ok(equipo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipo([FromBody] Equipo equipo)
        {
            if (equipo == null)
                return BadRequest();

            await _equipoService.CrearEquipo(equipo);
            return CreatedAtAction(nameof(GetEquipo), new { id = equipo.EquipoId }, equipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipo(int id, [FromBody] Equipo equipo)
        {
            if (equipo == null || id != equipo.EquipoId)
                return BadRequest();

            var equipoExistente = await _equipoService.ObtenerEquipoPorId(id);
            if (equipoExistente == null)
                return NotFound();

            await _equipoService.ActualizarEquipo(equipo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipo(int id)
        {
            var equipo = await _equipoService.ObtenerEquipoPorId(id);
            if (equipo == null)
                return NotFound();

            await _equipoService.EliminarEquipo(id);
            return NoContent();
        }
    }
}
