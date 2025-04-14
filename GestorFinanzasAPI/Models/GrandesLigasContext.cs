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
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<JugadorEquipo> JugadoresEquipos { get; set; }
        public DbSet<EntrenadorEquipo> EntrenadoresEquipos { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Resultado> Resultados { get; set; }
        public DbSet<Clasificacion> Clasificaciones { get; set; }
    }
}