using System.ComponentModel.DataAnnotations;

namespace GrandesLigasAPI.Models
{
    public class Equipo
    {
        [Key]
        public int EquipoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; }

        [Required]
        public int LigaId { get; set; }

        [Required]
        public byte[] Logo { get; set; }

        [Required]
        public byte[] FotoEstadio { get; set; }
    }
}
