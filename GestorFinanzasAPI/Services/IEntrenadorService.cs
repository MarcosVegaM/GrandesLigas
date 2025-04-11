// Services/IEntrenadorService.cs
using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IEntrenadorService
    {
        Task<IEnumerable<Entrenador>> ObtenerEntrenadores();
        Task<Entrenador> ObtenerEntrenadorPorId(int id);
        Task CrearEntrenador(Entrenador entrenador);
        Task ActualizarEntrenador(Entrenador entrenador);
        Task EliminarEntrenador(int id);
    }
}
