// Services/ResultadoService.cs
using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class ResultadoService : IResultadoService
    {
        private readonly GrandesLigasContext _context;

        public ResultadoService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resultado>> ObtenerResultados()
        {
            return await _context.Resultados
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Resultado> ObtenerResultadoPorId(int id)
        {
            return await _context.Resultados
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ResultadoId == id);
        }

        public async Task<IEnumerable<Resultado>> ObtenerResultadosPorPartido(int partidoId)
        {
            return await _context.Resultados
                .Where(r => r.PartidoId == partidoId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CrearResultado(Resultado resultado)
        {
            if (resultado == null)
                throw new ArgumentNullException(nameof(resultado));

            var partido = await _context.Partidos
                .FirstOrDefaultAsync(p => p.PartidoId == resultado.PartidoId);

            if (partido == null)
                throw new KeyNotFoundException("Partido no encontrado");

            _context.Resultados.Add(resultado);
            await _context.SaveChangesAsync();

            await ActualizarClasificaciones(resultado, partido);
        }

        public async Task ActualizarResultado(Resultado resultado)
        {
            if (resultado == null)
                throw new ArgumentNullException(nameof(resultado));

            var resultadoExistente = await _context.Resultados
                .FirstOrDefaultAsync(r => r.ResultadoId == resultado.ResultadoId);

            if (resultadoExistente == null)
                throw new KeyNotFoundException("Resultado no encontrado");

            var partido = await _context.Partidos
                .FirstOrDefaultAsync(p => p.PartidoId == resultadoExistente.PartidoId);

            if (partido == null)
                throw new KeyNotFoundException("Partido no encontrado");

            await RevertirClasificaciones(resultadoExistente, partido);

            _context.Entry(resultadoExistente).CurrentValues.SetValues(resultado);
            await _context.SaveChangesAsync();

            await ActualizarClasificaciones(resultadoExistente, partido);
        }

        public async Task EliminarResultado(int id)
        {
            var resultado = await _context.Resultados.FindAsync(id);
            if (resultado == null)
                throw new KeyNotFoundException("Resultado no encontrado");

            var partido = await _context.Partidos
                .FirstOrDefaultAsync(p => p.PartidoId == resultado.PartidoId);

            if (partido != null)
            {
                await RevertirClasificaciones(resultado, partido);
            }

            _context.Resultados.Remove(resultado);
            await _context.SaveChangesAsync();
        }

        private async Task ActualizarClasificaciones(Resultado resultado, Partido partido)
        {
            int? equipoGanadorId = null;
            if (resultado.CarrerasLocal > resultado.CarrerasVisitante)
                equipoGanadorId = partido.EquipoLocalId;
            else if (resultado.CarrerasVisitante > resultado.CarrerasLocal)
                equipoGanadorId = partido.EquipoVisitanteId;

            await ActualizarClasificacionEquipo(
                partido.LigaId,
                partido.EquipoLocalId,
                resultado.CarrerasLocal,
                resultado.CarrerasVisitante,
                equipoGanadorId == partido.EquipoLocalId);

            await ActualizarClasificacionEquipo(
                partido.LigaId,
                partido.EquipoVisitanteId,
                resultado.CarrerasVisitante,
                resultado.CarrerasLocal,
                equipoGanadorId == partido.EquipoVisitanteId);
        }

        private async Task RevertirClasificaciones(Resultado resultado, Partido partido)
        {
            int? equipoGanadorId = null;
            if (resultado.CarrerasLocal > resultado.CarrerasVisitante)
                equipoGanadorId = partido.EquipoLocalId;
            else if (resultado.CarrerasVisitante > resultado.CarrerasLocal)
                equipoGanadorId = partido.EquipoVisitanteId;

            await ActualizarClasificacionEquipo(
                partido.LigaId,
                partido.EquipoLocalId,
                -resultado.CarrerasLocal,
                -resultado.CarrerasVisitante,
                equipoGanadorId == partido.EquipoLocalId,
                revertir: true);

            await ActualizarClasificacionEquipo(
                partido.LigaId,
                partido.EquipoVisitanteId,
                -resultado.CarrerasVisitante,
                -resultado.CarrerasLocal,
                equipoGanadorId == partido.EquipoVisitanteId,
                revertir: true);
        }

        private async Task ActualizarClasificacionEquipo(
            int ligaId,
            int equipoId,
            int carrerasAnotadas,
            int carrerasRecibidas,
            bool esGanador,
            bool revertir = false)
        {
            var clasificacion = await _context.Clasificaciones
                .FirstOrDefaultAsync(c => c.LigaId == ligaId && c.EquipoId == equipoId);

            if (clasificacion == null)
            {
                if (revertir) return;

                clasificacion = new Clasificacion
                {
                    LigaId = ligaId,
                    EquipoId = equipoId,
                    JuegosJugados = 0,
                    JuegosGanados = 0,
                    JuegosPerdidos = 0,
                    CarrerasAnotadas = 0,
                    CarrerasRecibidas = 0,
                    Porcentaje = 0
                };
                _context.Clasificaciones.Add(clasificacion);
            }

            clasificacion.JuegosJugados += revertir ? -1 : 1;

            if (esGanador)
                clasificacion.JuegosGanados += revertir ? -1 : 1;
            else
                clasificacion.JuegosPerdidos += revertir ? -1 : 1;

            clasificacion.CarrerasAnotadas += carrerasAnotadas;
            clasificacion.CarrerasRecibidas += carrerasRecibidas;

            clasificacion.Porcentaje = clasificacion.JuegosJugados > 0
                ? (decimal)clasificacion.JuegosGanados / clasificacion.JuegosJugados
                : 0;

            await _context.SaveChangesAsync();
        }
    }
}