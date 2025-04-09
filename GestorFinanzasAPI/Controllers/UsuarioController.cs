using GrandesLigasAPI.Models;
using GrandesLigasAPI.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();

            await _usuarioService.CrearUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null || id != usuario.Id)
                return BadRequest();

            var usuarioExistente = await _usuarioService.ObtenerUsuarioPorId(id);
            if (usuarioExistente == null)
                return NotFound();

            await _usuarioService.ActualizarUsuario(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            await _usuarioService.EliminarUsuario(id);
            return NoContent();
        }
    }
}

