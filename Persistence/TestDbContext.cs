using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence;

public class TestDbContext : IdentityDbContext<AppUser>
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    public DbSet<Tarea> Tareas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tarea>().ToTable("tareas");

        LoadDataSecurity(modelBuilder);
    }

    private void LoadDataSecurity(ModelBuilder modelBuilder)
    {
        var adminId = Guid.NewGuid().ToString();
        var clientId = Guid.NewGuid().ToString();

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { 
                Id = adminId,
                Name = CustomRoles.ADMIN,
                NormalizedName = CustomRoles.ADMIN
            }
        );

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = clientId,
                Name = CustomRoles.CLIENT,
                NormalizedName = CustomRoles.CLIENT
            }
        );

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string> { 
                    Id = 1,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TAREA_READ,
                    RoleId = adminId,
                }
            );

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 2,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TAREA_UPDATE,
                    RoleId = adminId,
                }
            );

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 3,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TAREA_WRITE,
                    RoleId = adminId,
                }
            );

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 4,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TAREA_DELETE,
                    RoleId = adminId,
                }
            );

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 5,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.TAREA_READ,
                    RoleId = clientId,
                }
            );

    }
}
