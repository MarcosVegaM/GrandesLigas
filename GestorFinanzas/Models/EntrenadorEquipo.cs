using System;
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class EntrenadorEquipo
    {
        [Key]
        public int EntrenadorEquipoId { get; set; }

        [Required]
        public int EntrenadorId { get; set; }

        [Required]
        public int EquipoId { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaIngreso { get; set; }
    }
}
