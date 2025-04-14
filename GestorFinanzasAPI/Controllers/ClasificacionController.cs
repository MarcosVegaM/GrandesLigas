// Controllers/ClasificacionController.cs
using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasificacionController : ControllerBase
    {
        private readonly IClasificacionService _clasificacionService;

        public ClasificacionController(IClasificacionService clasificacionService)
        {
            _clasificacionService = clasificacionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClasificaciones()
        {
            var clasificaciones = await _clasificacionService.ObtenerClasificaciones();
            return Ok(clasificaciones);
        }

        [HttpGet("porliga/{ligaId}")]
        public async Task<IActionResult> GetClasificacionPorLiga(int ligaId)
        {
            var clasificaciones = await _clasificacionService.ObtenerClasificacionPorLiga(ligaId);
            return Ok(clasificaciones);
        }

        [HttpGet("porequipo/{ligaId}/{equipoId}")]
        public async Task<IActionResult> GetClasificacionPorEquipo(int ligaId, int equipoId)
        {
            var clasificacion = await _clasificacionService.ObtenerClasificacionPorEquipo(ligaId, equipoId);
            if (clasificacion == null)
                return NotFound();
            return Ok(clasificacion);
        }
    }
}
