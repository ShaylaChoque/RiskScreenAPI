using Microsoft.EntityFrameworkCore;
using RiskScreenAPI.Security.Domain.Models;
using RiskScreenAPI.Shared.Extensions;

namespace RiskScreenAPI.Shared.Persistence.Contexts;

//Provides access to the database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    //Entities
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //Constraints
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u=>u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(30);
        builder.Entity<User>().Property(u => u.Role).IsRequired();
        //Relationships
        
        //Naming convention
        builder.UseSnakeCaseNamingConvention();
    }
}