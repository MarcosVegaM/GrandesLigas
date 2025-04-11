using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class EquipoService : IEquipoService
    {
        private readonly GrandesLigasContext _context;

        public EquipoService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipo>> ObtenerEquipos()
        {
            return await _context.Equipos.ToListAsync();
        }

        public async Task<Equipo> ObtenerEquipoPorId(int id)
        {
            return await _context.Equipos.FindAsync(id);
        }

        public async Task CrearEquipo(Equipo equipo)
        {
            if (equipo == null)
            {
                throw new ArgumentNullException(nameof(equipo));
            }

            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEquipo(Equipo equipo)
        {
            // Buscar el equipo existente en la base de datos
            var equipoExistente = await _context.Equipos.FindAsync(equipo.EquipoId);
            if (equipoExistente == null)
            {
                throw new Exception("Equipo no encontrado");
            }

            // Actualizar propiedades
            equipoExistente.Nombre = equipo.Nombre;
            equipoExistente.Ciudad = equipo.Ciudad;
            equipoExistente.LigaId = equipo.LigaId;

            // Actualizar campos binarios solo si se proporcionan nuevos valores
            if (equipo.Logo != null && equipo.Logo.Length > 0)
            {
                equipoExistente.Logo = equipo.Logo;
            }

            if (equipo.FotoEstadio != null && equipo.FotoEstadio.Length > 0)
            {
                equipoExistente.FotoEstadio = equipo.FotoEstadio;
            }

            await _context.SaveChangesAsync();
        }

        public async Task EliminarEquipo(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
