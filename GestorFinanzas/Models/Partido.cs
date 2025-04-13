// Models/Partido.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class Partido
    {
        [Key]
        public int PartidoId { get; set; }

        [Required]
        public int LigaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(100)]
        public string Lugar { get; set; }

        [Required]
        public int EquipoLocalId { get; set; }

        [Required]
        public int EquipoVisitanteId { get; set; }
    }
}
