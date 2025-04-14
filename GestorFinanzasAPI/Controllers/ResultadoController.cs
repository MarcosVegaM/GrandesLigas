// Controllers/ResultadoController.cs
using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoController : ControllerBase
    {
        private readonly IResultadoService _resultadoService;

        public ResultadoController(IResultadoService resultadoService)
        {
            _resultadoService = resultadoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetResultados()
        {
            var resultados = await _resultadoService.ObtenerResultados();
            return Ok(resultados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResultado(int id)
        {
            var resultado = await _resultadoService.ObtenerResultadoPorId(id);
            if (resultado == null)
                return NotFound();
            return Ok(resultado);
        }

        [HttpGet("porpartido/{partidoId}")]
        public async Task<IActionResult> GetResultadosPorPartido(int partidoId)
        {
            var resultados = await _resultadoService.ObtenerResultadosPorPartido(partidoId);
            return Ok(resultados);
        }

        [HttpPost]
        public async Task<IActionResult> CreateResultado([FromBody] Resultado resultado)
        {
            if (resultado == null)
                return BadRequest();

            try
            {
                await _resultadoService.CrearResultado(resultado);
                return CreatedAtAction(nameof(GetResultado), new { id = resultado.ResultadoId }, resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResultado(int id, [FromBody] Resultado resultado)
        {
            if (resultado == null || id != resultado.ResultadoId)
                return BadRequest();

            try
            {
                await _resultadoService.ActualizarResultado(resultado);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResultado(int id)
        {
            try
            {
                await _resultadoService.EliminarResultado(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}