// Services/ClasificacionService.cs
using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class ClasificacionService : IClasificacionService
    {
        private readonly GrandesLigasContext _context;

        public ClasificacionService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clasificacion>> ObtenerClasificaciones()
        {
            return await _context.Clasificaciones
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Clasificacion>> ObtenerClasificacionPorLiga(int ligaId)
        {
            return await _context.Clasificaciones
                .Where(c => c.LigaId == ligaId)
                .OrderByDescending(c => c.Porcentaje)
                .ThenByDescending(c => c.CarrerasAnotadas - c.CarrerasRecibidas) // Diferencia de carreras
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Clasificacion> ObtenerClasificacionPorEquipo(int ligaId, int equipoId)
        {
            return await _context.Clasificaciones
                .Where(c => c.LigaId == ligaId && c.EquipoId == equipoId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}