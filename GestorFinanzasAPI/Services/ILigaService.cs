using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface ILigaService
    {
        Task<IEnumerable<Liga>> ObtenerLigas();
        Task<Liga> ObtenerLigaPorId(int id);
        Task CrearLiga(Liga liga);
        Task ActualizarLiga(Liga liga);
        Task EliminarLiga(int id);
    }
}

