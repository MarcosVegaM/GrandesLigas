using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class JugadorService : IJugadorService
    {
        private readonly GrandesLigasContext _context;

        public JugadorService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Jugador>> ObtenerJugadores()
        {
            return await _context.Jugadores
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Jugador> ObtenerJugadorPorId(int id)
        {
            return await _context.Jugadores
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.JugadorId == id);
        }

        public async Task CrearJugador(Jugador jugador)
        {
            if (jugador == null)
                throw new ArgumentNullException(nameof(jugador));

            _context.Jugadores.Add(jugador);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarJugador(Jugador jugador)
        {
            if (jugador == null)
                throw new ArgumentNullException(nameof(jugador));

            var jugadorExistente = await _context.Jugadores
                .FirstOrDefaultAsync(j => j.JugadorId == jugador.JugadorId);

            if (jugadorExistente == null)
                throw new KeyNotFoundException("Jugador no encontrado");

            // Usamos Entry para manejar mejor las propiedades
            _context.Entry(jugadorExistente).CurrentValues.SetValues(jugador);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarJugador(int id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null)
                throw new KeyNotFoundException("Jugador no encontrado");

            _context.Jugadores.Remove(jugador);
            await _context.SaveChangesAsync();
        }

    }
}
