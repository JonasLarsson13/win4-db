using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<StatusTypeEntity> StatusTypes { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CustomerContactEntity> CustomerContacts { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.StatusType)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.ProjectLeader)
            .WithMany(e => e.ProjectsAsLeader)
            .HasForeignKey(p => p.ProjectLeaderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.Products)
            .WithMany(p => p.Projects)
            .UsingEntity(j => j.ToTable("ProjectProducts"));
        
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.Customers)
            .WithMany(c => c.Projects)
            .UsingEntity(j => j.ToTable("ProjectCustomers"));
        
        modelBuilder.Entity<CustomerEntity>()
            .HasOne(c => c.Contact)
            .WithOne()
            .HasForeignKey<CustomerEntity>(c => c.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}