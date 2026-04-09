using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IAuditInterceptor
{
}

public class AuditInterceptor : SaveChangesInterceptor, IAuditInterceptor
{
    private static readonly string[] AddedByPropertyNames = ["AddedBy", "CreatedBy"];
    private static readonly string[] AddedOnPropertyNames = ["AddedOn", "CreatedDate"];
    private static readonly string[] ModifiedByPropertyNames = ["ModifiedBy", "UpdatedBy"];
    private static readonly string[] ModifiedOnPropertyNames = ["ModifiedOn", "UpdatedDate"];

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(currentUser))
        {
            currentUser = "system";
        }

        var now = DateTime.UtcNow;

        foreach (var entry in dbContext.ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            ApplyAuditValues(entry, currentUser, now);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyAuditValues(EntityEntry entry, string currentUser, DateTime now)
    {
        if (entry.State == EntityState.Added)
        {
            SetStringPropertyIfPresent(entry, AddedByPropertyNames, currentUser);
            SetDatePropertyIfPresent(entry, AddedOnPropertyNames, now);
        }

        if (entry.State == EntityState.Modified)
        {
            SetStringPropertyIfPresent(entry, ModifiedByPropertyNames, currentUser);
            SetDatePropertyIfPresent(entry, ModifiedOnPropertyNames, now);
        }
    }

    private static void SetStringPropertyIfPresent(EntityEntry entry, IEnumerable<string> candidateNames, string value)
    {
        var property = FindProperty(entry, candidateNames);
        if (property is null)
        {
            return;
        }

        if (property.Metadata.ClrType == typeof(string))
        {
            property.CurrentValue = value;
        }
    }

    private static void SetDatePropertyIfPresent(EntityEntry entry, IEnumerable<string> candidateNames, DateTime value)
    {
        var property = FindProperty(entry, candidateNames);
        if (property is null)
        {
            return;
        }

        if (property.Metadata.ClrType == typeof(DateTime))
        {
            property.CurrentValue = value;
            return;
        }

        if (property.Metadata.ClrType == typeof(DateTime?))
        {
            property.CurrentValue = value;
        }
    }

    private static PropertyEntry? FindProperty(EntityEntry entry, IEnumerable<string> candidateNames)
    {
        return entry.Properties.FirstOrDefault(property =>
            candidateNames.Any(candidate =>
                string.Equals(property.Metadata.Name, candidate, StringComparison.OrdinalIgnoreCase)));
    }
}
