using Microsoft.EntityFrameworkCore;
using Stargate.API.Data.Entities;

namespace Stargate.API.Data
{
    public class StargateContext : DbContext
    {
        public StargateContext(DbContextOptions<StargateContext> options) : base(options) { }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasIndex(b => b.FileName)
                .IsUnique();
        }
    }
}