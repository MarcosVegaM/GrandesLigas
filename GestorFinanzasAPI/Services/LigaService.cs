using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public class LigaService : ILigaService
    {
        private readonly GrandesLigasContext _context;

        public LigaService(GrandesLigasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Liga>> ObtenerLigas()
        {
            return await _context.Ligas.ToListAsync();
        }

        public async Task<Liga> ObtenerLigaPorId(int id)
        {
            return await _context.Ligas.FindAsync(id);
        }

        public async Task CrearLiga(Liga liga)
        {
            _context.Ligas.Add(liga);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarLiga(Liga liga)
        {
            // Primero buscar la liga existente en la base de datos
            var ligaExistente = await _context.Ligas.FindAsync(liga.Id);
            if (ligaExistente == null)
            {
                throw new Exception("Liga no encontrada");
            }

            // Si la liga existe, actualiza sus propiedades
            ligaExistente.Nombre = liga.Nombre;
            ligaExistente.Descripcion = liga.Descripcion;
            ligaExistente.Temporada = liga.Temporada;
            // Otros campos que quieras actualizar...

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        public async Task EliminarLiga(int id)
        {
            var liga = await _context.Ligas.FindAsync(id);
            if (liga != null)
            {
                _context.Ligas.Remove(liga);
                await _context.SaveChangesAsync();
            }
        }
    }
}
