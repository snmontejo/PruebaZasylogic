using Microsoft.EntityFrameworkCore;
using PruebaZasylogic.Models;

namespace PruebaZasylogic.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Motivo> Motivos { get; set; }
        public DbSet<AtencionCliente> AtencionesCliente { get; set; }
    }
}