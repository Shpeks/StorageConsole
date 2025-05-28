using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }
    
    public DbSet<Box> Boxes { get; set; }
    public DbSet<Pallet> Pallets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Box>(entity =>
        {
            entity.HasOne(b => b.Pallet)
                .WithMany(p => p.Boxes)
                .HasForeignKey(b => b.PalletId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}