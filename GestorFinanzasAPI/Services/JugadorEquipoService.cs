using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class JugadorEquipoService : IJugadorEquipoService
    {
        private readonly GrandesLigasContext _context;

        public JugadorEquipoService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JugadorEquipo>> ObtenerJugadoresEquipos()
        {
            return await _context.JugadoresEquipos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<JugadorEquipo> ObtenerJugadorEquipoPorId(int id)
        {
            return await _context.JugadoresEquipos
                .AsNoTracking()
                .FirstOrDefaultAsync(je => je.JugadorEquipoId == id);
        }

        public async Task<bool> ExisteRelacion(int jugadorId, int equipoId)
        {
            return await _context.JugadoresEquipos
                .AsNoTracking()
                .AnyAsync(je => je.JugadorId == jugadorId && je.EquipoId == equipoId);
        }

        public async Task CrearJugadorEquipo(JugadorEquipo jugadorEquipo)
        {
            if (jugadorEquipo == null)
                throw new ArgumentNullException(nameof(jugadorEquipo));

            // Validar existencia de jugador y equipo
            var jugadorExiste = await _context.Jugadores.AnyAsync(j => j.JugadorId == jugadorEquipo.JugadorId);
            var equipoExiste = await _context.Equipos.AnyAsync(e => e.EquipoId == jugadorEquipo.EquipoId);

            if (!jugadorExiste || !equipoExiste)
                throw new Exception("Jugador o Equipo no existe");

            if (!jugadorEquipo.FechaIngreso.HasValue)
                jugadorEquipo.FechaIngreso = DateTime.Today;

            _context.JugadoresEquipos.Add(jugadorEquipo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarJugadorEquipo(JugadorEquipo jugadorEquipo)
        {
            var existente = await _context.JugadoresEquipos
                .FirstOrDefaultAsync(je => je.JugadorEquipoId == jugadorEquipo.JugadorEquipoId);

            if (existente == null)
                throw new Exception("Relación no encontrada");

            existente.JugadorId = jugadorEquipo.JugadorId;
            existente.EquipoId = jugadorEquipo.EquipoId;
            existente.FechaIngreso = jugadorEquipo.FechaIngreso;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarJugadorEquipo(int id)
        {
            var relacion = await _context.JugadoresEquipos.FindAsync(id);
            if (relacion != null)
            {
                _context.JugadoresEquipos.Remove(relacion);
                await _context.SaveChangesAsync();
            }
        }

        // Métodos alternativos para obtener datos relacionados
        public async Task<IEnumerable<int>> ObtenerJugadoresIdsPorEquipo(int equipoId)
        {
            return await _context.JugadoresEquipos
                .Where(je => je.EquipoId == equipoId)
                .Select(je => je.JugadorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> ObtenerEquiposIdsPorJugador(int jugadorId)
        {
            return await _context.JugadoresEquipos
                .Where(je => je.JugadorId == jugadorId)
                .Select(je => je.EquipoId)
                .ToListAsync();
        }
    }
}