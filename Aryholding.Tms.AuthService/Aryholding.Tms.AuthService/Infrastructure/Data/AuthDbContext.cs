using Aryholding.Tms.AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.AuthService.Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<UserDepartmentRole> UserDepartmentRoles { get; set; }
        public DbSet<PriorityLevel> PriorityLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasQueryFilter(u => u.IsActive);
            builder.Entity<Department>().HasQueryFilter(d => d.IsActive);
            builder.Entity<UserDepartmentRole>().HasQueryFilter(udr => udr.IsActive);
            builder.Entity<PriorityLevel>().HasQueryFilter(pl => pl.IsActive);
        }
    }
}
