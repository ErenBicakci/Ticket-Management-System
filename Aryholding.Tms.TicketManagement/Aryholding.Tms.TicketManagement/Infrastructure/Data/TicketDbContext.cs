using Aryholding.Tms.TicketManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Data
{
    public class TicketDbContext : IdentityDbContext<ApplicationUser>
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<PriorityLevel> PriorityLevels { get; set; }
        public DbSet<UserDepartmentRole> UserDepartmentRoles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketSeverity> TicketSeverities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketEventType> TicketEventTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<Department>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<PriorityLevel>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<UserDepartmentRole>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<Ticket>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<TicketCategory>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<Comment>().HasQueryFilter(e => e.IsActive);

            modelBuilder.Entity<TicketEventType>().HasData(
                new TicketEventType { Id = 1, EventTypeCode = "CREATED", DisplayName = "Oluşturuldu" },
                new TicketEventType { Id = 2, EventTypeCode = "STATUS_CHANGED", DisplayName = "Durum Değişti" },
                new TicketEventType { Id = 3, EventTypeCode = "PRIORITY_CHANGED", DisplayName = "Öncelik Değişti" },
                new TicketEventType { Id = 4, EventTypeCode = "ASSIGNEE_CHANGED", DisplayName = "Atanan Kişi Değişti" },
                new TicketEventType { Id = 5, EventTypeCode = "TITLE_CHANGED", DisplayName = "İçerik Değişti" },
                new TicketEventType { Id = 6, EventTypeCode = "ATTACHMENT_ADDED", DisplayName = "Ek Eklendi" },
                new TicketEventType { Id = 7, EventTypeCode = "ATTACHMENT_REMOVED", DisplayName = "Ek Kaldırıldı" },
                new TicketEventType { Id = 8, EventTypeCode = "REOPENED", DisplayName = "Yeniden Açıldı" },
                new TicketEventType { Id = 9, EventTypeCode = "SYSTEM_NOTE", DisplayName = "Sistem Notu" },
                new TicketEventType { Id = 10, EventTypeCode = "DELETED", DisplayName = "Silindi" },
                new TicketEventType { Id = 11, EventTypeCode = "UNASSIGNED", DisplayName = "Atama Kaldırıldı" },
                new TicketEventType { Id = 12, EventTypeCode = "UPDATED", DisplayName = "Güncellendi" }
            );

            modelBuilder.Entity<TicketSeverity>().HasData(
                new TicketSeverity { Id = 1, TicketSeverityCode = "INFO", Name = "Info" },
                new TicketSeverity { Id = 2, TicketSeverityCode = "LOW", Name = "Low" },
                new TicketSeverity { Id = 3, TicketSeverityCode = "MEDIUM", Name = "Medium" },
                new TicketSeverity { Id = 4, TicketSeverityCode = "HIGH", Name = "High" },
                new TicketSeverity { Id = 5, TicketSeverityCode = "CRITICAL", Name = "Critical" }
            );

            modelBuilder.Entity<TicketStatus>().HasData(
                new TicketStatus { Id = 1, Name = "Beklemede", TicketStatusCode = "PENDING" },
                new TicketStatus { Id = 2, Name = "Kabul Edildi", TicketStatusCode = "ACCEPTED" },
                new TicketStatus { Id = 3, Name = "Onay Bekliyor", TicketStatusCode = "WAITING_APPROVAL" },
                new TicketStatus { Id = 4, Name = "Onaylandı", TicketStatusCode = "APPROVED" }
            );

            modelBuilder.Entity<TicketHistory>(builder =>
            {
                builder.HasKey(h => h.Id);
                builder.Property(h => h.EventMessage)
                       .HasColumnName("eventMessage")
                       .IsRequired(false);
                builder.HasOne(h => h.OldStatus)
                       .WithMany()
                       .HasForeignKey(h => h.OldStatusId)
                       .OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(h => h.NewStatus)
                       .WithMany()
                       .HasForeignKey(h => h.NewStatusId)
                       .OnDelete(DeleteBehavior.Restrict);
                builder.HasIndex(h => new { h.TicketId, h.ChangedAt });
                builder.HasIndex(h => h.NewStatusId);
            });
        }
    }
}
