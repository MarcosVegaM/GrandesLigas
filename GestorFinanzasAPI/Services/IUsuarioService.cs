using GrandesLigasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrandesLigasAPI.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObtenerUsuarios();
        Task<Usuario> ObtenerUsuarioPorId(int id);
        Task CrearUsuario(Usuario usuario);
        Task ActualizarUsuario(Usuario usuario);
        Task EliminarUsuario(int id);
    }
}

