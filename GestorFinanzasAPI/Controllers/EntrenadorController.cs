// Controllers/EntrenadorController.cs
using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenadorController : ControllerBase
    {
        private readonly IEntrenadorService _entrenadorService;

        public EntrenadorController(IEntrenadorService entrenadorService)
        {
            _entrenadorService = entrenadorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntrenadores()
        {
            var entrenadores = await _entrenadorService.ObtenerEntrenadores();
            return Ok(entrenadores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntrenador(int id)
        {
            var entrenador = await _entrenadorService.ObtenerEntrenadorPorId(id);
            if (entrenador == null) return NotFound();
            return Ok(entrenador);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntrenador([FromBody] Entrenador entrenador)
        {
            if (entrenador == null) return BadRequest();
            await _entrenadorService.CrearEntrenador(entrenador);
            return CreatedAtAction(nameof(GetEntrenador), new { id = entrenador.EntrenadorId }, entrenador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntrenador(int id, [FromBody] Entrenador entrenador)
        {
            if (entrenador == null || id != entrenador.EntrenadorId) return BadRequest();
            var entrenadorExistente = await _entrenadorService.ObtenerEntrenadorPorId(id);
            if (entrenadorExistente == null) return NotFound();
            await _entrenadorService.ActualizarEntrenador(entrenador);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrenador(int id)
        {
            var entrenador = await _entrenadorService.ObtenerEntrenadorPorId(id);
            if (entrenador == null) return NotFound();
            await _entrenadorService.EliminarEntrenador(id);
            return NoContent();
        }
    }
}
