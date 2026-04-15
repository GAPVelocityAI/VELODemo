using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Helpers;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OrderStatusEnum = Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models.Enums.OrderStatus;
using SystemSettingsEnum = Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models.Enums.SystemSettings;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class ModOrdersService : IModOrdersService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly IModDaoService _modDaoService;
    private readonly IModStartupService _modStartupService;
    private readonly ILogger<ModOrdersService> _logger;

    public ModOrdersService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        IModDaoService modDaoService,
        IModStartupService modStartupService,
        ILogger<ModOrdersService> logger)
    {
        _dbFactory = dbFactory;
        _modDaoService = modDaoService;
        _modStartupService = modStartupService;
        _logger = logger;
    }

    public int GetRandom(int lowerBound, int upperBound)
    {
        if (upperBound < lowerBound)
        {
            throw new ArgumentOutOfRangeException(nameof(upperBound), "Upper bound must be greater than or equal to lower bound.");
        }

        return Random.Shared.Next(lowerBound, upperBound + 1);
    }

    public async Task<string> CreateRandomOrdersAsync(short orderCount, CancellationToken cancellationToken = default)
    {
        if (orderCount <= 0)
        {
            return "No orders were created because order count was less than or equal to zero.";
        }

        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var latestOrderId = 0;

        for (var orderCounter = 1; orderCounter <= orderCount; orderCounter++)
        {
            var order = new Orders
            {
                EmployeeID = AppConstants.InternetSalesEmployeeId,
                CustomerID = await GetRandomCustomerIDAsync(cancellationToken),
                OrderDate = DateTime.Now,
                StatusID = (int)OrderStatusEnum.New,
                Notes = "Internet Order",
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync(cancellationToken);

            latestOrderId = order.OrderID;

            var detailsToCreate = GetRandom(2, 5);
            for (var detailCounter = 2; detailCounter <= detailsToCreate; detailCounter++)
            {
                await AddOrderDetailWithRetryAsync(db, latestOrderId, cancellationToken);
            }
        }

        // TODO: DoCmd.RunMacro("macMainMenu_UpdateSubs") uses Access UI/macros — implement an app-specific equivalent if needed.

        var firstOrderId = latestOrderId - orderCount + 1;
        return $"Orders created. The new OrderIDs are from {firstOrderId} to {latestOrderId}.";
    }

    public async Task SetDatesToCurrentAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var maxOrderDate = await db.Orders.Select(o => (DateTime?)o.OrderDate).MaxAsync(cancellationToken) ?? DateTime.Today;
        var dayDelta = (DateTime.Today - maxOrderDate.Date).Days;

        if (dayDelta < 0)
        {
            return;
        }

        foreach (var entityType in db.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (string.IsNullOrWhiteSpace(tableName) || tableName.StartsWith("MSys", StringComparison.OrdinalIgnoreCase) || tableName.StartsWith("USys", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var tableIdentifier = StoreObjectIdentifier.Table(tableName, entityType.GetSchema());
            var dateProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

            foreach (var dateProperty in dateProperties)
            {
                var columnName = dateProperty.GetColumnName(tableIdentifier);
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    continue;
                }

                var sql = $"UPDATE [{tableName}] SET [{columnName}] = DATEADD(day, {dayDelta}, [{columnName}]) WHERE [{columnName}] IS NOT NULL";
                await db.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            }
        }

        await _modStartupService.SaveSystemSettingAsync(SystemSettingsEnum.LastResetDate, DateTime.Today, cancellationToken);
    }

    private async Task<int> GetRandomCustomerIDAsync(CancellationToken cancellationToken)
    {
        return await _modDaoService.GetRandomPkValueAsync("Customers", "CustomerID", cancellationToken);
    }

    private async Task<int> GetRandomProductIDAsync(CancellationToken cancellationToken)
    {
        return await _modDaoService.GetRandomPkValueAsync("Products", "ProductID", cancellationToken);
    }

    private async Task AddOrderDetailWithRetryAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db,
        int orderId,
        CancellationToken cancellationToken)
    {
        const int maxAttempts = 20;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var productId = await GetRandomProductIDAsync(cancellationToken);
            var unitPrice = await db.Products
                .Where(p => p.ProductID == productId)
                .Select(p => (decimal?)p.UnitPrice)
                .FirstOrDefaultAsync(cancellationToken) ?? 0m;

            var detail = new OrderDetails
            {
                OrderID = orderId,
                ProductID = productId,
                Quantity = (short)GetRandom(5, 50),
                UnitPrice = unitPrice,
            };

            db.OrderDetails.Add(detail);

            try
            {
                await db.SaveChangesAsync(cancellationToken);
                return;
            }
            catch (DbUpdateException updateException) when (IsDuplicateKey(updateException))
            {
                _logger.LogDebug(updateException, "Duplicate order detail encountered on attempt {Attempt}. Retrying.", attempt);
                db.Entry(detail).State = EntityState.Detached;
            }
        }

        throw new InvalidOperationException($"Unable to create an order detail for order {orderId} after {maxAttempts} attempts.");
    }

    private static bool IsDuplicateKey(DbUpdateException exception)
    {
        var message = exception.InnerException?.Message ?? exception.Message;

        return message.Contains("duplicate", StringComparison.OrdinalIgnoreCase)
               || message.Contains("unique", StringComparison.OrdinalIgnoreCase)
               || message.Contains("2627", StringComparison.OrdinalIgnoreCase)
               || message.Contains("2601", StringComparison.OrdinalIgnoreCase);
    }
}
