using GerenciadorCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorCursos.Data
{
    public class GerenciadorContext : DbContext
    {
        public GerenciadorContext(DbContextOptions<GerenciadorContext> options) : base(options)
        {
        }

        public DbSet<CursosModel> CursosModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CursosModel>(e =>
            {
                e.ToTable("Cursos");

                e.HasKey(p => p.Id);

                e.Property(f => f.Titulo).HasColumnType("varchar(40)").IsRequired();

                e.Property(g => g.Duracao).HasColumnType("varchar(40)");

                e.Property(h => h.Status).HasColumnType("nvarchar(max)");

            });
        }



    }
}
