using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class EntrenadorService : IEntrenadorService
    {
        private readonly GrandesLigasContext _context;

        public EntrenadorService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entrenador>> ObtenerEntrenadores()
        {
            return await _context.Entrenadores
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Entrenador> ObtenerEntrenadorPorId(int id)
        {
            return await _context.Entrenadores
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EntrenadorId == id);
        }

        public async Task CrearEntrenador(Entrenador entrenador)
        {
            if (entrenador == null)
                throw new ArgumentNullException(nameof(entrenador));

            _context.Entrenadores.Add(entrenador);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEntrenador(Entrenador entrenador)
        {
            if (entrenador == null)
                throw new ArgumentNullException(nameof(entrenador));

            var entrenadorExistente = await _context.Entrenadores
                .FirstOrDefaultAsync(e => e.EntrenadorId == entrenador.EntrenadorId);

            if (entrenadorExistente == null)
                throw new KeyNotFoundException("Entrenador no encontrado");

            // Usamos Entry para manejar mejor las propiedades
            _context.Entry(entrenadorExistente).CurrentValues.SetValues(entrenador);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarEntrenador(int id)
        {
            var entrenador = await _context.Entrenadores.FindAsync(id);
            if (entrenador == null)
                throw new KeyNotFoundException("Entrenador no encontrado");

            _context.Entrenadores.Remove(entrenador);
            await _context.SaveChangesAsync();
        }
    }
}
