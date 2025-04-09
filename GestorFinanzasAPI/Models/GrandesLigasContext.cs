using GrandesLigasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GrandesLigasAPI.Models
{
    public class GrandesLigasContext : DbContext
    {
        public GrandesLigasContext(DbContextOptions<GrandesLigasContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Liga> Ligas { get; set; }
    }
}