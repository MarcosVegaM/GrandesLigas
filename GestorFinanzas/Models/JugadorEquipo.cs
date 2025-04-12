using System;
using System.ComponentModel.DataAnnotations;

namespace GrandesLigas.Models
{
    public class JugadorEquipo
    {
        [Key]
        public int JugadorEquipoId { get; set; }

        [Required]
        public int JugadorId { get; set; }

        [Required]
        public int EquipoId { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaIngreso { get; set; }
    }
}
