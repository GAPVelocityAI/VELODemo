using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IModDAOService
{
    Task<int> GetRandomPkValueAsync(string tableName, string pkField, CancellationToken cancellationToken = default);
    bool HasField(string tableName, string fieldName);
    bool HasProperty(object instance, string propertyName);
}

public class ModDAOService : IModDAOService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;

    public ModDAOService(IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<int> GetRandomPkValueAsync(string tableName, string pkField, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var values = await ((tableName.Trim(), pkField.Trim()) switch
        {
            ("Customers", "CustomerID") => db.Customers.Select(c => c.CustomerID).ToListAsync(cancellationToken),
            ("Employees", "EmployeeID") => db.Employees.Select(e => e.EmployeeID).ToListAsync(cancellationToken),
            ("NorthwindFeatures", "NorthwindFeaturesID") => db.NorthwindFeatures.Select(n => n.NorthwindFeaturesID).ToListAsync(cancellationToken),
            ("OrderDetails", "OrderDetailID") => db.OrderDetails.Select(od => od.OrderDetailID).ToListAsync(cancellationToken),
            ("OrderStatus", "StatusID") => db.OrderStatus.Select(os => os.StatusID).ToListAsync(cancellationToken),
            ("Orders", "OrderID") => db.Orders.Select(o => o.OrderID).ToListAsync(cancellationToken),
            ("Products", "ProductID") => db.Products.Select(p => p.ProductID).ToListAsync(cancellationToken),
            ("SystemSettings", "SettingID") => db.SystemSettings.Select(ss => ss.SettingID).ToListAsync(cancellationToken),
            ("Welcome", "ID") => db.Welcome.Select(w => w.ID).ToListAsync(cancellationToken),
            _ => throw new ArgumentException($"Unsupported table/pk combination: {tableName}.{pkField}"),
        });

        if (values.Count == 0)
        {
            throw new InvalidOperationException($"Table {tableName} does not contain any records.");
        }

        var randomIndex = Random.Shared.Next(0, values.Count);
        return values[randomIndex];
    }

    public bool HasField(string tableName, string fieldName)
    {
        using var db = _dbFactory.CreateDbContext();

        var normalizedTable = tableName.Trim();
        var normalizedField = fieldName.Trim();

        var entityType = db.Model.GetEntityTypes().FirstOrDefault(et =>
            string.Equals(et.GetTableName(), normalizedTable, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(et.ClrType.Name, normalizedTable, StringComparison.OrdinalIgnoreCase));

        if (entityType is null)
        {
            return false;
        }

        return entityType.GetProperties().Any(p => string.Equals(p.Name, normalizedField, StringComparison.OrdinalIgnoreCase));
    }

    public bool HasProperty(object instance, string propertyName)
    {
        var property = instance.GetType().GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        return property is not null;
    }
}
