using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IJugadorEquipoService
    {
        Task<IEnumerable<JugadorEquipo>> ObtenerJugadoresEquipos();
        Task<JugadorEquipo> ObtenerJugadorEquipoPorId(int id);
        Task<bool> ExisteRelacion(int jugadorId, int equipoId);
        Task CrearJugadorEquipo(JugadorEquipo jugadorEquipo);
        Task ActualizarJugadorEquipo(JugadorEquipo jugadorEquipo);
        Task EliminarJugadorEquipo(int id);
        Task<IEnumerable<int>> ObtenerJugadoresIdsPorEquipo(int equipoId);
        Task<IEnumerable<int>> ObtenerEquiposIdsPorJugador(int jugadorId);
    }
}
