// Models/Entrenador.cs
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class Entrenador
    {
        [Key]
        public int EntrenadorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Especialidad { get; set; }
    }
}
