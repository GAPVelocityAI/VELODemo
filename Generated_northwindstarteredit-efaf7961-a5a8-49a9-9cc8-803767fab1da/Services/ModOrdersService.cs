using Microsoft.EntityFrameworkCore;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Helpers;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using OrderStatusEnum = Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models.Enums.OrderStatus;
using SystemSettingsEnum = Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models.Enums.SystemSettings;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IModOrdersService
{
    Task CreateRandomOrdersAsync(short orderCount, CancellationToken cancellationToken = default);
    int GetRandom(int lowerBound, int upperBound);
    Task SetDatesToCurrentAsync(CancellationToken cancellationToken = default);
}

public class ModOrdersService : IModOrdersService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly IModDAOService _modDAOService;
    private readonly IModStartupService _modStartupService;

    public ModOrdersService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        IModDAOService modDAOService,
        IModStartupService modStartupService)
    {
        _dbFactory = dbFactory;
        _modDAOService = modDAOService;
        _modStartupService = modStartupService;
    }

    public async Task CreateRandomOrdersAsync(short orderCount, CancellationToken cancellationToken = default)
    {
        if (orderCount <= 0)
        {
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var lastOrderId = 0;

        for (var orderIndex = 1; orderIndex <= orderCount; orderIndex++)
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

            lastOrderId = order.OrderID;

            var detailsCount = GetRandom(2, 5);
            for (var detailIndex = 2; detailIndex <= detailsCount; detailIndex++)
            {
                var saved = false;

                while (!saved)
                {
                    var productId = await GetRandomProductIDAsync(cancellationToken);
                    var unitPrice = await db.Products
                        .Where(p => p.ProductID == productId)
                        .Select(p => (decimal?)p.UnitPrice)
                        .FirstOrDefaultAsync(cancellationToken) ?? 0m;

                    var detail = new OrderDetails
                    {
                        OrderID = lastOrderId,
                        ProductID = productId,
                        Quantity = (short)GetRandom(5, 50),
                        UnitPrice = unitPrice,
                    };

                    try
                    {
                        db.OrderDetails.Add(detail);
                        await db.SaveChangesAsync(cancellationToken);
                        saved = true;
                    }
                    catch (DbUpdateException ex) when (IsDuplicateKeyException(ex))
                    {
                        db.Entry(detail).State = EntityState.Detached;
                    }
                }
            }
        }

        // TODO: VBA called DoCmd.RunMacro("macMainMenu_UpdateSubs") here (Access UI macro).
        var _ = $"Orders created. The new OrderIDs are from {lastOrderId - orderCount + 1} to {lastOrderId}.";
    }

    public int GetRandom(int lowerBound, int upperBound)
    {
        if (upperBound < lowerBound)
        {
            throw new ArgumentException("Upper bound must be greater than or equal to lower bound.");
        }

        return Random.Shared.Next(lowerBound, upperBound + 1);
    }

    public async Task SetDatesToCurrentAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var maxOrderDate = await db.Orders.MaxAsync(o => (DateTime?)o.OrderDate, cancellationToken) ?? DateTime.Today;
        var deltaDays = (DateTime.Today - maxOrderDate.Date).Days;

        if (deltaDays < 0)
        {
            return;
        }

        var customers = await db.Customers.ToListAsync(cancellationToken);
        foreach (var customer in customers)
        {
            customer.AddedOn = ShiftNullableDate(customer.AddedOn, deltaDays);
            customer.ModifiedOn = ShiftNullableDate(customer.ModifiedOn, deltaDays);
        }

        var employees = await db.Employees.ToListAsync(cancellationToken);
        foreach (var employee in employees)
        {
            employee.AddedOn = ShiftNullableDate(employee.AddedOn, deltaDays);
            employee.ModifiedOn = ShiftNullableDate(employee.ModifiedOn, deltaDays);
        }

        var orderStatuses = await db.OrderStatus.ToListAsync(cancellationToken);
        foreach (var orderStatus in orderStatuses)
        {
            orderStatus.AddedOn = ShiftNullableDate(orderStatus.AddedOn, deltaDays);
            orderStatus.ModifiedOn = ShiftNullableDate(orderStatus.ModifiedOn, deltaDays);
        }

        var orders = await db.Orders.ToListAsync(cancellationToken);
        foreach (var order in orders)
        {
            order.OrderDate = order.OrderDate.AddDays(deltaDays);
            order.ShippedDate = ShiftNullableDate(order.ShippedDate, deltaDays);
            order.PaidDate = ShiftNullableDate(order.PaidDate, deltaDays);
            order.AddedOn = ShiftNullableDate(order.AddedOn, deltaDays);
            order.ModifiedOn = ShiftNullableDate(order.ModifiedOn, deltaDays);
        }

        var orderDetails = await db.OrderDetails.ToListAsync(cancellationToken);
        foreach (var orderDetail in orderDetails)
        {
            orderDetail.AddedOn = ShiftNullableDate(orderDetail.AddedOn, deltaDays);
            orderDetail.ModifiedOn = ShiftNullableDate(orderDetail.ModifiedOn, deltaDays);
        }

        var products = await db.Products.ToListAsync(cancellationToken);
        foreach (var product in products)
        {
            product.AddedOn = ShiftNullableDate(product.AddedOn, deltaDays);
            product.ModifiedOn = ShiftNullableDate(product.ModifiedOn, deltaDays);
        }

        await db.SaveChangesAsync(cancellationToken);

        await _modStartupService.SaveSystemSettingAsync(SystemSettingsEnum.LastResetDate, DateTime.Today, cancellationToken);
    }

    private Task<int> GetRandomCustomerIDAsync(CancellationToken cancellationToken)
    {
        return _modDAOService.GetRandomPkValueAsync("Customers", "CustomerID", cancellationToken);
    }

    private Task<int> GetRandomProductIDAsync(CancellationToken cancellationToken)
    {
        return _modDAOService.GetRandomPkValueAsync("Products", "ProductID", cancellationToken);
    }

    private static DateTime? ShiftNullableDate(DateTime? value, int dayOffset)
    {
        return value?.AddDays(dayOffset);
    }

    private static bool IsDuplicateKeyException(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message;

        return message is not null &&
               (message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("unique", StringComparison.OrdinalIgnoreCase));
    }
}
