
using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GrandesLigasAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly GrandesLigasContext _context;

        public UsuarioService(GrandesLigasContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<Usuario>> ObtenerUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // Obtener un usuario por su ID
        public async Task<Usuario> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            return usuario;
        }

        // Crear un nuevo usuario
        public async Task CrearUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        // Actualizar un usuario existente
        public async Task ActualizarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
            if (usuarioExistente == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            // Actualizar propiedades del usuario
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Contraseña = usuario.Contraseña; // Considera hashear la contraseña
            

            _context.Usuarios.Update(usuarioExistente);
            await _context.SaveChangesAsync();
        }

        // Eliminar un usuario por su ID
        public async Task EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }
        }
    }
}
