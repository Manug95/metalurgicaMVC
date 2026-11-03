using metalurgicaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Trabajo> Trabajos { get; set; }
    public DbSet<Foto> Fotos { get; set; }
    public DbSet<Material> Materiales { get; set; }
    public DbSet<TipoOperacion> TipoOperaciones { get; set; }
    public DbSet<Detalle> Detalles { get; set; }
}