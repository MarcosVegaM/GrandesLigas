using GrandesLigasAPI.Models;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IClasificacionService
    {
        Task<IEnumerable<Clasificacion>> ObtenerClasificaciones();
        Task<IEnumerable<Clasificacion>> ObtenerClasificacionPorLiga(int ligaId);
        Task<Clasificacion> ObtenerClasificacionPorEquipo(int ligaId, int equipoId);
    }
}
