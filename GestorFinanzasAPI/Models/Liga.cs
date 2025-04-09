using System.ComponentModel.DataAnnotations;

namespace GrandesLigasAPI.Models
{
    public class Liga
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string? Temporada { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }
    }
}

