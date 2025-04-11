using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly IJugadorService _jugadorService;

        public JugadorController(IJugadorService jugadorService)
        {
            _jugadorService = jugadorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJugadores()
        {
            var jugadores = await _jugadorService.ObtenerJugadores();
            return Ok(jugadores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJugador(int id)
        {
            var jugador = await _jugadorService.ObtenerJugadorPorId(id);
            if (jugador == null)
                return NotFound();
            return Ok(jugador);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJugador([FromBody] Jugador jugador)
        {
            if (jugador == null)
                return BadRequest();

            await _jugadorService.CrearJugador(jugador);
            return CreatedAtAction(nameof(GetJugador), new { id = jugador.JugadorId }, jugador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJugador(int id, [FromBody] Jugador jugador)
        {
            if (jugador == null || id != jugador.JugadorId)
                return BadRequest();

            var jugadorExistente = await _jugadorService.ObtenerJugadorPorId(id);
            if (jugadorExistente == null)
                return NotFound();

            await _jugadorService.ActualizarJugador(jugador);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJugador(int id)
        {
            var jugador = await _jugadorService.ObtenerJugadorPorId(id);
            if (jugador == null)
                return NotFound();

            await _jugadorService.EliminarJugador(id);
            return NoContent();
        }
    }
}
