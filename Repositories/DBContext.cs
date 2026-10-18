using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     // Configuración del ValueConverter
    //     var tipoMaterialConverter = new ValueConverter<TipoMaterial, string>(
    //         v => v.ToString(),              // De enum a string (para guardar en BD)
    //         v => Enum.Parse<TipoMaterial>(v)      // De string a enum (para leer de BD)
    //     );

    //     modelBuilder.Entity<Material>()
    //         .Property(m => m.Tipo)
    //         .HasConversion(tipoMaterialConverter)
    //         .HasColumnType("ENUM('SIN_TIPO', 'CHAPA', 'CAÑO', 'PINTURA')");

    //     var estadoTrabajoConverter = new ValueConverter<EstadoTrabajo, string>(
    //         v => v.ToString(),              // De enum a string (para guardar en BD)
    //         v => Enum.Parse<EstadoTrabajo>(v)      // De string a enum (para leer de BD)
    //     );

    //     modelBuilder.Entity<Trabajo>()
    //         .Property(m => m.Estado)
    //         .HasConversion(estadoTrabajoConverter)
    //         .HasColumnType("ENUM('PRESUPUESTO','ACEPTADO','RECHAZADO')");

    //     //ESTO ES PARA GUARDAR LOS NOMBRES DE LOS VALORES DE UN ENUM EN LUGAR DEL VALOR NUMERICO
    //     // modelBuilder.Entity<Trabajo>()
    //     //     .Property(t => t.Estado)
    //     //     .HasConversion<string>(); // 👈 Esto convierte el enum a texto
    //     // modelBuilder.Entity<Pago>()
    //     //     .Property(t => t.FormaPago)
    //     //     .HasConversion<string>();
    // }

}