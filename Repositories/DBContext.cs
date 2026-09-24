using metalurgicaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    // public DbSet<Trabajo> Trabajos { get; set; }
    // public DbSet<Pago> Pagos { get; set; }
    // public DbSet<Foto> Fotos { get; set; }
    // public DbSet<Gasto> Gastos { get; set; }
    // public DbSet<TipoGasto> TipoGasto { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     //ESTO ES PARA GUARDAR LOS NOMBRES DE LOS VALORES DE UN ENUM EN LUGAR DEL VALOR NUMERICO
    //     modelBuilder.Entity<Trabajo>()
    //         .Property(t => t.Estado)
    //         .HasConversion<string>(); // 👈 Esto convierte el enum a texto
    //     modelBuilder.Entity<Pago>()
    //         .Property(t => t.FormaPago)
    //         .HasConversion<string>();
    // }

}