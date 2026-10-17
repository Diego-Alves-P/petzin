using Microsoft.EntityFrameworkCore;
using Petzin.Api.Models;

namespace Petzin.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<HistoricoMedico> Historicos => Set<HistoricoMedico>();
    public DbSet<Canil> Canis => Set<Canil>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>(entidade =>
        {
            entidade.Property(t => t.Nome).HasMaxLength(120).IsRequired();
            entidade.Property(t => t.Telefone).HasMaxLength(20).IsRequired();
            entidade.Property(t => t.Email).HasMaxLength(160);
        });

        modelBuilder.Entity<Pet>(entidade =>
        {
            entidade.Property(p => p.Nome).HasMaxLength(120).IsRequired();
            entidade.Property(p => p.Especie).HasMaxLength(40).IsRequired();
            entidade.Property(p => p.Raca).HasMaxLength(80).IsRequired();
            entidade.HasOne(p => p.Tutor)
                .WithMany(t => t.Pets)
                .HasForeignKey(p => p.TutorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HistoricoMedico>(entidade =>
        {
            entidade.Property(h => h.Descricao).HasMaxLength(500).IsRequired();
            entidade.HasOne(h => h.Pet)
                .WithMany(p => p.Historicos)
                .HasForeignKey(h => h.PetId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Canil>(entidade =>
        {
            entidade.Property(c => c.Porte).HasMaxLength(20).IsRequired();
            entidade.HasIndex(c => c.Numero).IsUnique();
        });

        modelBuilder.Entity<Reserva>(entidade =>
        {
            entidade.Property(r => r.ValorDiaria).HasPrecision(10, 2);
            entidade.Property(r => r.ValorTotal).HasPrecision(10, 2);
            entidade.Property(r => r.Status).HasMaxLength(20).IsRequired();
            entidade.HasOne(r => r.Pet)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PetId)
                .OnDelete(DeleteBehavior.Restrict);
            entidade.HasOne(r => r.Canil)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.CanilId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
