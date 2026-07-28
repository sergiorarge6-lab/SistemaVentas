using Microsoft.EntityFrameworkCore;
using SistemaVentas.Domain.Entities;

namespace SistemaVentas.Infrastructure.Data;

public class SistemaVentasDbContext : DbContext
{
    public SistemaVentasDbContext(
        DbContextOptions<SistemaVentasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<PedidoCabecera> PedidoCabeceras => Set<PedidoCabecera>();

    public DbSet<PedidoDetalle> PedidoDetalles => Set<PedidoDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================
        // Producto
        // ==========================
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Productos");
            entity.HasKey(p => p.Id);
        });

        // ==========================
        // PedidoCabecera
        // ==========================
        modelBuilder.Entity<PedidoCabecera>(entity =>
        {
            entity.ToTable("PedidoCabecera");
            entity.HasKey(c => c.Id);
        });

        // ==========================
        // PedidoDetalle
        // ==========================
            modelBuilder.Entity<PedidoDetalle>(entity =>
            {

            entity.ToTable("PedidoDetalle");

            entity.HasKey(d => d.Id);

            entity
                // Cardinalidad
                .HasOne(d => d.PedidoCabecera).WithMany(c => c.Detalles)
                // Columnas que participan de la relación
                .HasForeignKey(d => d.PedidoCabeceraId).HasPrincipalKey(c => c.Id);

            entity
                .HasOne(d => d.Producto).WithMany()
                .HasForeignKey(d => d.ProductoId).HasPrincipalKey(p => p.Id);
        });
    }
}