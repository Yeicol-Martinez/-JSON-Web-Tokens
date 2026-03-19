using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Models;

namespace UsuariosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario>       Usuarios       { get; set; }
        public DbSet<CuentaUsuario> CuentasUsuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

            modelBuilder.Entity<CuentaUsuario>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder.Entity<CuentaUsuario>()
                .HasOne(c => c.Usuario)
                .WithOne()
                .HasForeignKey<CuentaUsuario>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
