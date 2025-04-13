using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class EntrenadorEquipoService : IEntrenadorEquipoService
    {
        private readonly GrandesLigasContext _context;

        public EntrenadorEquipoService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EntrenadorEquipo>> ObtenerEntrenadoresEquipos()
        {
            return await _context.EntrenadoresEquipos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EntrenadorEquipo> ObtenerEntrenadorEquipoPorId(int id)
        {
            return await _context.EntrenadoresEquipos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EntrenadorEquipoId == id);
        }

        public async Task<bool> ExisteRelacion(int entrenadorId, int equipoId)
        {
            return await _context.EntrenadoresEquipos
                .AsNoTracking()
                .AnyAsync(e => e.EntrenadorId == entrenadorId && e.EquipoId == equipoId);
        }

        public async Task CrearEntrenadorEquipo(EntrenadorEquipo entrenadorEquipo)
        {
            if (entrenadorEquipo == null)
                throw new ArgumentNullException(nameof(entrenadorEquipo));

            // Validar existencia de entrenador y equipo
            var entrenadorExiste = await _context.Entrenadores.AnyAsync(e => e.EntrenadorId == entrenadorEquipo.EntrenadorId);
            var equipoExiste = await _context.Equipos.AnyAsync(e => e.EquipoId == entrenadorEquipo.EquipoId);

            if (!entrenadorExiste || !equipoExiste)
                throw new Exception("Entrenador o Equipo no existe");

            if (!entrenadorEquipo.FechaIngreso.HasValue)
                entrenadorEquipo.FechaIngreso = DateTime.Today;

            _context.EntrenadoresEquipos.Add(entrenadorEquipo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEntrenadorEquipo(EntrenadorEquipo entrenadorEquipo)
        {
            var existente = await _context.EntrenadoresEquipos
                .FirstOrDefaultAsync(e => e.EntrenadorEquipoId == entrenadorEquipo.EntrenadorEquipoId);

            if (existente == null)
                throw new Exception("Relación no encontrada");

            existente.EntrenadorId = entrenadorEquipo.EntrenadorId;
            existente.EquipoId = entrenadorEquipo.EquipoId;
            existente.FechaIngreso = entrenadorEquipo.FechaIngreso;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarEntrenadorEquipo(int id)
        {
            var relacion = await _context.EntrenadoresEquipos.FindAsync(id);
            if (relacion != null)
            {
                _context.EntrenadoresEquipos.Remove(relacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<int>> ObtenerEntrenadoresIdsPorEquipo(int equipoId)
        {
            return await _context.EntrenadoresEquipos
                .Where(e => e.EquipoId == equipoId)
                .Select(e => e.EntrenadorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> ObtenerEquiposIdsPorEntrenador(int entrenadorId)
        {
            return await _context.EntrenadoresEquipos
                .Where(e => e.EntrenadorId == entrenadorId)
                .Select(e => e.EquipoId)
                .ToListAsync();
        }
    }
}
