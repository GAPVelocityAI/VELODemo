using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class ModDaoService : IModDaoService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;

    public ModDaoService(IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<int> GetRandomPkValueAsync(string tableName, string pkField, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var normalizedTableName = tableName.Trim().ToLowerInvariant();
        var normalizedFieldName = pkField.Trim().ToLowerInvariant();

        var values = await ((normalizedTableName, normalizedFieldName) switch
        {
            ("customers", "customerid") => db.Customers.Select(c => c.CustomerID).ToListAsync(cancellationToken),
            ("employees", "employeeid") => db.Employees.Select(e => e.EmployeeID).ToListAsync(cancellationToken),
            ("northwindfeatures", "northwindfeaturesid") => db.NorthwindFeatures.Select(n => n.NorthwindFeaturesID).ToListAsync(cancellationToken),
            ("orderdetails", "orderdetailid") => db.OrderDetails.Select(od => od.OrderDetailID).ToListAsync(cancellationToken),
            ("orderstatus", "statusid") => db.OrderStatus.Select(os => os.StatusID).ToListAsync(cancellationToken),
            ("orders", "orderid") => db.Orders.Select(o => o.OrderID).ToListAsync(cancellationToken),
            ("products", "productid") => db.Products.Select(p => p.ProductID).ToListAsync(cancellationToken),
            ("systemsettings", "settingid") => db.SystemSettings.Select(s => s.SettingID).ToListAsync(cancellationToken),
            ("welcome", "id") => db.Welcome.Select(w => w.ID).ToListAsync(cancellationToken),
            _ => throw new ArgumentException($"Unsupported table/field combination: {tableName}.{pkField}", nameof(tableName)),
        });

        if (values.Count == 0)
        {
            throw new InvalidOperationException($"No records found in table '{tableName}'.");
        }

        var randomIndex = Random.Shared.Next(0, values.Count);
        return values[randomIndex];
    }

    public bool HasField(string tableName, string fieldName)
    {
        using var db = _dbFactory.CreateDbContext();

        var entityType = db.Model.GetEntityTypes()
            .FirstOrDefault(et =>
                string.Equals(et.GetTableName(), tableName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(et.ClrType.Name, tableName, StringComparison.OrdinalIgnoreCase));

        if (entityType is null)
        {
            return false;
        }

        return entityType.GetProperties()
            .Any(p => string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase));
    }

    public bool HasProperty(object target, string propertyName)
    {
        return target.GetType().GetProperty(propertyName) is not null;
    }
}
