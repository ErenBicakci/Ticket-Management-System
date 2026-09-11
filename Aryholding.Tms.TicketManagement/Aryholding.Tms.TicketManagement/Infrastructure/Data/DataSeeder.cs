using Aryholding.Tms.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(TicketDbContext context, ILogger? logger = null)
    {
        await SeedDepartmentsAsync(context, logger);
        await SeedPriorityLevelsAsync(context, logger);
    }

    public static async Task SeedDepartmentsAsync(TicketDbContext context, ILogger? logger = null)
    {
        var existingCodes = await context.Departments
            .IgnoreQueryFilters()
            .Select(d => d.DepartmentCode)
            .ToListAsync();

        var departmentsToSeed = new List<Department>
        {
            new() { DepartmentName = "Bilgi Teknolojileri", DepartmentCode = "IT", IsActive = true },
            new() { DepartmentName = "Yazılım Geliştirme", DepartmentCode = "DEV", IsActive = true },
            new() { DepartmentName = "Sistem ve Ağ Yönetimi", DepartmentCode = "SYSNET", IsActive = true },
            new() { DepartmentName = "Siber Güvenlik", DepartmentCode = "CYBER", IsActive = true },
            new() { DepartmentName = "İnsan Kaynakları", DepartmentCode = "HR", IsActive = true },
            new() { DepartmentName = "Muhasebe ve Finans", DepartmentCode = "FIN", IsActive = true },
            new() { DepartmentName = "Satın Alma ve Tedarik", DepartmentCode = "PROC", IsActive = true },
            new() { DepartmentName = "Hukuk Müşavirliği", DepartmentCode = "LEGAL", IsActive = true },
            new() { DepartmentName = "Operasyon ve Lojistik", DepartmentCode = "OPS", IsActive = true },
            new() { DepartmentName = "Satış ve Pazarlama", DepartmentCode = "SALES", IsActive = true },
            new() { DepartmentName = "Müşteri İlişkileri ve Destek", DepartmentCode = "SUPPORT", IsActive = true },
            new() { DepartmentName = "İdari İşler", DepartmentCode = "ADMIN", IsActive = true },
            new() { DepartmentName = "Kalite Yönetimi ve Denetim", DepartmentCode = "QA", IsActive = true },
            new() { DepartmentName = "Ar-Ge ve İnovasyon", DepartmentCode = "RND", IsActive = true },
            new() { DepartmentName = "Proje Yönetim Ofisi (PMO)", DepartmentCode = "PMO", IsActive = true },
            new() { DepartmentName = "Tesis ve Bina Yönetimi", DepartmentCode = "FACILITY", IsActive = true },
            new() { DepartmentName = "Kurumsal İletişim", DepartmentCode = "CORPCOMM", IsActive = true },
            new() { DepartmentName = "İş Geliştirme ve Strateji", DepartmentCode = "STRATEGY", IsActive = true }
        };

        var now = DateTime.UtcNow;
        var toAdd = departmentsToSeed
            .Where(d => !existingCodes.Contains(d.DepartmentCode))
            .Select(d =>
            {
                d.CreatedAt = now;
                d.UpdatedAt = now;
                return d;
            })
            .ToList();

        if (toAdd.Count > 0)
        {
            await context.Departments.AddRangeAsync(toAdd);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} departments successfully.", toAdd.Count);
        }
    }

    public static async Task SeedPriorityLevelsAsync(TicketDbContext context, ILogger? logger = null)
    {
        var existingCodes = await context.PriorityLevels
            .IgnoreQueryFilters()
            .Select(p => p.PriorityLevelCode)
            .ToListAsync();

        var priorityLevelsToSeed = new List<PriorityLevel>
        {
            new() { Level = 1, Name = "Stajyer", PriorityLevelCode = "INTERN", IsActive = true },
            new() { Level = 2, Name = "Personel", PriorityLevelCode = "STAFF", IsActive = true },
            new() { Level = 3, Name = "Kıdemli Personel", PriorityLevelCode = "SENIOR_STAFF", IsActive = true },
            new() { Level = 4, Name = "Uzman", PriorityLevelCode = "SPECIALIST", IsActive = true },
            new() { Level = 5, Name = "Kıdemli Uzman", PriorityLevelCode = "SENIOR_SPECIALIST", IsActive = true },
            new() { Level = 6, Name = "Ekip Lideri", PriorityLevelCode = "TEAM_LEAD", IsActive = true },
            new() { Level = 7, Name = "Departman Yöneticisi", PriorityLevelCode = "DEPT_MANAGER", IsActive = true },
            new() { Level = 8, Name = "Kıdemli Yönetici", PriorityLevelCode = "SENIOR_MANAGER", IsActive = true },
            new() { Level = 9, Name = "Direktör", PriorityLevelCode = "DIRECTOR", IsActive = true },
            new() { Level = 10, Name = "Departman Sahibi", PriorityLevelCode = "OWNER", IsActive = true }
        };

        var now = DateTime.UtcNow;
        var toAdd = priorityLevelsToSeed
            .Where(p => !existingCodes.Contains(p.PriorityLevelCode))
            .Select(p =>
            {
                p.CreatedAt = now;
                p.UpdatedAt = now;
                return p;
            })
            .ToList();

        if (toAdd.Count > 0)
        {
            await context.PriorityLevels.AddRangeAsync(toAdd);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} priority levels successfully.", toAdd.Count);
        }
    }
}
