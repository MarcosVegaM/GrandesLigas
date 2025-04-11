using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IEquipoService
    {
        Task<IEnumerable<Equipo>> ObtenerEquipos();
        Task<Equipo> ObtenerEquipoPorId(int id);
        Task CrearEquipo(Equipo equipo);
        Task ActualizarEquipo(Equipo equipo);
        Task EliminarEquipo(int id);
    }
}
