using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UsuariosAPI.Data
{
    /// <summary>
    /// Factory usado por las herramientas de EF Core en tiempo de diseño
    /// (Add-Migration, Update-Database) para crear el DbContext
    /// sin necesitar levantar toda la aplicación.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-YEICOL\\SQLEXPRESS;Database=UsuariosDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
