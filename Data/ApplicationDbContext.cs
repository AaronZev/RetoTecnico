using Microsoft.EntityFrameworkCore;
using RetoTecnico.Model;
using System.Threading.Tasks;

namespace RetoTecnico.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Categorias>? Categorias { get; set; }
        public DbSet<Roles>? Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
