using Consumer.Models;
using Microsoft.EntityFrameworkCore;

public class ProtocoloContext : DbContext
{
    public DbSet<Protocolo> Protocolos { get; set; }

    public ProtocoloContext(DbContextOptions<ProtocoloContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura a entidade Protocolo
        modelBuilder.Entity<Protocolo>()
            .HasIndex(p => p.NumeroProtocolo)
            .IsUnique();

        modelBuilder.Entity<Protocolo>()
            .HasIndex(p => new { p.Cpf, p.Rg, p.NumeroVia })
            .IsUnique();
    }
}