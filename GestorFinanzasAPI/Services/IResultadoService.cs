// Services/IResultadoService.cs
using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IResultadoService
    {
        Task<IEnumerable<Resultado>> ObtenerResultados();
        Task<Resultado> ObtenerResultadoPorId(int id);
        Task<IEnumerable<Resultado>> ObtenerResultadosPorPartido(int partidoId);
        Task CrearResultado(Resultado resultado);
        Task ActualizarResultado(Resultado resultado);
        Task EliminarResultado(int id);
    }
}
