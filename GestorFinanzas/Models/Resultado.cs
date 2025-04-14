// Models/Resultado.cs
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class Resultado
    {
        [Key]
        public int ResultadoId { get; set; }

        [Required]
        public int PartidoId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CarrerasLocal { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CarrerasVisitante { get; set; }
    }
}
