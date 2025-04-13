// Services/PartidoService.cs
using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class PartidoService : IPartidoService
    {
        private readonly GrandesLigasContext _context;

        public PartidoService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Partido>> ObtenerPartidos()
        {
            return await _context.Partidos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Partido> ObtenerPartidoPorId(int id)
        {
            return await _context.Partidos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PartidoId == id);
        }
        public async Task<IEnumerable<Partido>> ObtenerPartidosPorLiga(int ligaId)
        {
            return await _context.Partidos
                .Where(p => p.LigaId == ligaId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task CrearPartido(Partido partido)
        {
            if (partido == null)
                throw new ArgumentNullException(nameof(partido));

            _context.Partidos.Add(partido);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarPartido(Partido partido)
        {
            if (partido == null)
                throw new ArgumentNullException(nameof(partido));

            var partidoExistente = await _context.Partidos
                .FirstOrDefaultAsync(p => p.PartidoId == partido.PartidoId);

            if (partidoExistente == null)
                throw new KeyNotFoundException("Partido no encontrado");

            _context.Entry(partidoExistente).CurrentValues.SetValues(partido);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarPartido(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null)
                throw new KeyNotFoundException("Partido no encontrado");

            _context.Partidos.Remove(partido);
            await _context.SaveChangesAsync();
        }
    }
}