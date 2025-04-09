using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class RestablecerContraseñaModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; }
    }
}
