using Microsoft.EntityFrameworkCore;
using RiskScreenAPI.Security.Domain.Models;
using RiskScreenAPI.Shared.Extensions;
using RiskScreenAPI.Web.Domain.Model;

namespace RiskScreenAPI.Shared.Persistence.Contexts;

//Provides access to the database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    //Entities
    public DbSet<User> Users { get; set; }
    public DbSet<Provider> Providers { get; set; }
    
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
        builder.Entity<User>().HasMany(u => u.Providers).WithOne(p => p.User).HasForeignKey(p => p.UserId);

        builder.Entity<Provider>().ToTable("Providers");
        builder.Entity<Provider>().HasKey(p => p.Id);
        builder.Entity<Provider>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Provider>().Property(p => p.LegalName).IsRequired().HasMaxLength(30);
        builder.Entity<Provider>().Property(p => p.CommercialName).IsRequired().HasMaxLength(30);
        builder.Entity<Provider>().Property(p => p.TaxIdentificationNumber).IsRequired();
        builder.Entity<Provider>().Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);
        builder.Entity<Provider>().Property(p => p.Email).IsRequired().HasMaxLength(30);
        builder.Entity<Provider>().Property(p => p.Website).IsRequired().HasMaxLength(30);
        builder.Entity<Provider>().Property(p => p.PhysicalAddress).IsRequired().HasMaxLength(50);
        builder.Entity<Provider>().Property(p => p.Country).IsRequired().HasMaxLength(30);
        builder.Entity<Provider>().Property(p => p.AnnualBillingInDollars).IsRequired();
        builder.Entity<Provider>().Property(p => p.LastEdited).IsRequired();
        
        //Naming convention
        builder.UseSnakeCaseNamingConvention();
    }
}