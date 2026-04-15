using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class AuditInterceptor : SaveChangesInterceptor, IAuditInterceptor
{
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
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var now = DateTime.UtcNow;
        var currentUser = ResolveCurrentUser();

        foreach (var entry in eventData.Context.ChangeTracker.Entries()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            if (entry.State == EntityState.Added)
            {
                SetAuditValue(entry, "AddedBy", currentUser);
                SetAuditValue(entry, "AddedOn", now);
                SetAuditValue(entry, "CreatedBy", currentUser);
                SetAuditValue(entry, "CreatedDate", now);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetAuditValue(entry, "ModifiedBy", currentUser);
                SetAuditValue(entry, "ModifiedOn", now);
                SetAuditValue(entry, "UpdatedBy", currentUser);
                SetAuditValue(entry, "UpdatedDate", now);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private string ResolveCurrentUser()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name
               ?? Environment.UserName
               ?? "system";
    }

    private static void SetAuditValue(EntityEntry entry, string propertyName, object value)
    {
        var propertyEntry = entry.Properties
            .FirstOrDefault(p => string.Equals(p.Metadata.Name, propertyName, StringComparison.OrdinalIgnoreCase));

        if (propertyEntry is not null)
        {
            propertyEntry.CurrentValue = value;
        }
    }
}
