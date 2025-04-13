// Services/IPartidoService.cs
using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IPartidoService
    {
        Task<IEnumerable<Partido>> ObtenerPartidos();
        Task<IEnumerable<Partido>> ObtenerPartidosPorLiga(int ligaId);
        Task<Partido> ObtenerPartidoPorId(int id);
        Task CrearPartido(Partido partido);
        Task ActualizarPartido(Partido partido);
        Task EliminarPartido(int id);
    }
}
