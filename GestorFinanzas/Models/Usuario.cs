using System;
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [StringLength(255, ErrorMessage = "El correo electrónico no puede exceder los 255 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, ErrorMessage = "La contraseña no puede exceder los 255 caracteres")]
        public string Contraseña { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        // Nueva propiedad para el token de restablecimiento
        public string? TokenRestablecimiento { get; set; }
        public int? Tipo { get; set; }
    }
}
