using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IEntrenadorEquipoService
    {
        Task<IEnumerable<EntrenadorEquipo>> ObtenerEntrenadoresEquipos();
        Task<EntrenadorEquipo> ObtenerEntrenadorEquipoPorId(int id);
        Task<bool> ExisteRelacion(int entrenadorId, int equipoId);
        Task CrearEntrenadorEquipo(EntrenadorEquipo entrenadorEquipo);
        Task ActualizarEntrenadorEquipo(EntrenadorEquipo entrenadorEquipo);
        Task EliminarEntrenadorEquipo(int id);
        Task<IEnumerable<int>> ObtenerEntrenadoresIdsPorEquipo(int equipoId);
        Task<IEnumerable<int>> ObtenerEquiposIdsPorEntrenador(int entrenadorId);
    }
}
