using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class OrderService : IOrderService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<OrderService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryActiveCustomers (distinct CustomerID where StatusID &lt;&gt; 1).
    /// </summary>
    public async Task<List<ActiveCustomerDto>> GetActiveCustomersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Orders
                .Where(o => o.StatusID != 1)
                .GroupBy(o => o.CustomerID)
                .Select(g => new ActiveCustomerDto
                {
                    CustomerID = g.Key,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryActiveCustomers.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryDistinctProductsThisWeek using ISO week boundaries.
    /// </summary>
    public async Task<List<DistinctProductsThisWeekDto>> GetDistinctProductsThisWeekAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            var today = DateTime.Today;
            var isoYear = ISOWeek.GetYear(today);
            var isoWeek = ISOWeek.GetWeekOfYear(today);
            var weekStart = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday);
            var weekEndExclusive = weekStart.AddDays(7);

            var orderIds = await (
                from o in db.Orders
                join od in db.OrderDetails on o.OrderID equals od.OrderID into odGroup
                from od in odGroup.DefaultIfEmpty()
                where o.OrderDate >= weekStart && o.OrderDate < weekEndExclusive
                select o.OrderID)
                .Distinct()
                .ToListAsync();

            return orderIds
                .Select(id => new DistinctProductsThisWeekDto { OrderID = id })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryDistinctProductsThisWeek.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrder (Orders.* with joined OrderStatus.StatusName).
    /// </summary>
    public async Task<List<OrderWithStatusDto>> GetOrderAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await (
                from o in db.Orders
                join s in db.OrderStatus on o.StatusID equals s.StatusID
                orderby o.OrderID
                select new OrderWithStatusDto
                {
                    OrderID = o.OrderID,
                    EmployeeID = o.EmployeeID,
                    CustomerID = o.CustomerID,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate,
                    PaidDate = o.PaidDate,
                    Notes = o.Notes,
                    StatusID = o.StatusID,
                    AddedBy = o.AddedBy,
                    AddedOn = o.AddedOn,
                    ModifiedBy = o.ModifiedBy,
                    ModifiedOn = o.ModifiedOn,
                    StatusName = s.StatusName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrder.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders and inlines sub-query references qryOrderTotal and qrycboEmployees.
    /// </summary>
    public async Task<List<OrderSummaryDto>> GetOrdersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildOrdersQuery(db).OrderByDescending(o => o.OrderID).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrders.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_Customer.
    /// </summary>
    public async Task<List<OrderSummaryDto>> GetOrdersMostRecentByCustomerAsync(int customerId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildOrdersQuery(db)
                .Where(o => o.CustomerID == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrders_MostRecent_Customer for CustomerID {CustomerId}.", customerId);
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_Employee.
    /// </summary>
    public async Task<List<OrderSummaryDto>> GetOrdersMostRecentByEmployeeAsync(int employeeId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildOrdersQuery(db)
                .Where(o => o.EmployeeID == employeeId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrders_MostRecent_Employee for EmployeeID {EmployeeId}.", employeeId);
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_ModifiedOn.
    /// </summary>
    public async Task<List<OrderSummaryDto>> GetOrdersMostRecentByModifiedOnAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildOrdersQuery(db)
                .OrderByDescending(o => o.ModifiedOn)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrders_MostRecent_ModifiedOn.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_OrderDate.
    /// </summary>
    public async Task<List<OrderSummaryDto>> GetOrdersMostRecentByOrderDateAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildOrdersQuery(db)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrders_MostRecent_OrderDate.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryProductOrders.
    /// </summary>
    public async Task<List<ProductOrderDto>> GetProductOrdersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await (
                from od in db.OrderDetails
                join o in db.Orders on od.OrderID equals o.OrderID
                join p in db.Products on od.ProductID equals p.ProductID
                select new ProductOrderDto
                {
                    OrderID = od.OrderID,
                    ProductID = od.ProductID,
                    OrderDate = o.OrderDate,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    ExtendedPrice = od.Quantity * od.UnitPrice,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryProductOrders.");
            throw;
        }
    }

    public async Task<List<Orders>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Orders.OrderByDescending(o => o.OrderID).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all orders.");
            throw;
        }
    }

    public async Task<Orders?> GetByIdAsync(int orderId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Orders.FirstOrDefaultAsync(o => o.OrderID == orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order {OrderId}.", orderId);
            throw;
        }
    }

    public async Task<Orders> CreateAsync(Orders order)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order.");
            throw;
        }
    }

    public async Task<Orders> UpdateAsync(Orders order)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Orders.Update(order);
            await db.SaveChangesAsync();
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order {OrderId}.", order.OrderID);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int orderId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Orders.Where(o => o.OrderID == orderId).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting order {OrderId}.", orderId);
            throw;
        }
    }

    private static IQueryable<OrderSummaryDto> BuildOrdersQuery(NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext db)
    {
        var qryOrderTotal = db.OrderDetails
            .GroupBy(od => od.OrderID)
            .Select(g => new
            {
                OrderID = g.Key,
                OrderTotal = g.Sum(x => x.Quantity * x.UnitPrice),
            });

        var qryCboEmployees = db.Employees
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                e.EmployeeID,
                e.FullNameFNLN,
            });

        return
            from o in db.Orders
            join ce in qryCboEmployees on o.EmployeeID equals ce.EmployeeID
            join c in db.Customers on o.CustomerID equals c.CustomerID
            join s in db.OrderStatus on o.StatusID equals s.StatusID
            join ot in qryOrderTotal on o.OrderID equals ot.OrderID into totalGroup
            from ot in totalGroup.DefaultIfEmpty()
            select new OrderSummaryDto
            {
                OrderID = o.OrderID,
                EmployeeID = o.EmployeeID,
                CustomerID = o.CustomerID,
                OrderDate = o.OrderDate,
                ShippedDate = o.ShippedDate,
                PaidDate = o.PaidDate,
                Notes = o.Notes,
                StatusID = o.StatusID,
                AddedBy = o.AddedBy,
                AddedOn = o.AddedOn,
                ModifiedBy = o.ModifiedBy,
                ModifiedOn = o.ModifiedOn,
                FullNameFNLN = ce.FullNameFNLN,
                CustomerName = c.CustomerName,
                StatusName = s.StatusName,
                OrderTotal = ot != null ? ot.OrderTotal : 0m,
                PlainTextNotes = o.Notes ?? string.Empty,
            };
    }
}
