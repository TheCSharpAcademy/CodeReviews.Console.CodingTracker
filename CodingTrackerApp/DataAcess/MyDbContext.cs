using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAcess;

public class MyDbContext : DbContext
{
    public DbSet<CodingSession> codingSessions {  get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.; Database=CodingTracker; Trusted_Connection=True; TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CodingSession>().HasKey(x => x.CodingSessionID);
    }
}
