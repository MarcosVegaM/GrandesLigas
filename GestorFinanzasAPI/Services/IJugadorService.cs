using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IJugadorService
    {
        Task<IEnumerable<Jugador>> ObtenerJugadores();
        Task<Jugador> ObtenerJugadorPorId(int id);
        Task CrearJugador(Jugador jugador);
        Task ActualizarJugador(Jugador jugador);
        Task EliminarJugador(int id);
    }
}
