// Models/Clasificacion.cs
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class Clasificacion
    {
        [Key]
        public int ClasificacionId { get; set; }

        [Required]
        public int LigaId { get; set; }

        [Required]
        public int EquipoId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int JuegosJugados { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int JuegosGanados { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int JuegosPerdidos { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CarrerasAnotadas { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CarrerasRecibidas { get; set; }

        [Required]
        [Range(0.0, 1.0)]
        public decimal Porcentaje { get; set; } // Nota: Corregí el nombre de "Procentaje" a "Porcentaje"
    }
}
