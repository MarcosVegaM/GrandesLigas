using GrandesLigas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GrandesLigas.Models
{
    public class GrandesLigasContext : DbContext
    {
        public GrandesLigasContext(DbContextOptions<GrandesLigasContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
