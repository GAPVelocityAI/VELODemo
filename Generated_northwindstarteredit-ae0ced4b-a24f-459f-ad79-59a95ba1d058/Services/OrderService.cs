using System.Globalization;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class ActiveCustomerDto
{
    public int CustomerID { get; set; }
}

public class CustomerProductSummaryDto
{
    public int CustomerID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int OrderCount { get; set; }
    public DateTime FirstOrdered { get; set; }
    public DateTime LastOrdered { get; set; }
}

public class DistinctProductsThisWeekDto
{
    public int OrderID { get; set; }
}

public class OrderListDto
{
    public int OrderID { get; set; }
    public int EmployeeID { get; set; }
    public int CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? Notes { get; set; }
    public int StatusID { get; set; }
    public string? AddedBy { get; set; }
    public DateTime? AddedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string FullNameFNLN { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public decimal? OrderTotal { get; set; }
    public string PlainTextNotes { get; set; } = string.Empty;
}

public class ProductOrderDto
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public DateTime OrderDate { get; set; }
    public short Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ExtendedPrice { get; set; }
}

public sealed class OrderService : IOrderService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<OrderService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryActiveCustomers: active customers from orders with StatusID <> 1.
    /// </summary>
    public async Task<List<ActiveCustomerDto>> GetActiveCustomersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Orders
                .AsNoTracking()
                .Where(o => o.StatusID != 1)
                .GroupBy(o => o.CustomerID)
                .Select(g => new ActiveCustomerDto { CustomerID = g.Key })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryActiveCustomers.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryCustomerProductSummary: per-customer product aggregates.
    /// </summary>
    public async Task<List<CustomerProductSummaryDto>> GetCustomerProductSummaryAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await (
                from o in db.Orders.AsNoTracking()
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID
                join p in db.Products.AsNoTracking() on od.ProductID equals p.ProductID
                group new { o, od, p } by new { o.CustomerID, p.ProductID, p.ProductName }
                into g
                select new CustomerProductSummaryDto
                {
                    CustomerID = g.Key.CustomerID,
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(x => x.od.Quantity),
                    OrderCount = g.Count(),
                    FirstOrdered = g.Min(x => x.o.OrderDate.Date),
                    LastOrdered = g.Max(x => x.o.OrderDate.Date),
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryCustomerProductSummary.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryDistinctProductsThisWeek: order ids for orders in the current ISO week.
    /// </summary>
    public async Task<List<DistinctProductsThisWeekDto>> GetDistinctProductsThisWeekAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var today = DateTime.Today;
            var currentYear = today.Year;
            var currentIsoWeek = ISOWeek.GetWeekOfYear(today);

            var rows = await (
                from o in db.Orders.AsNoTracking()
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID into odGroup
                from od in odGroup.DefaultIfEmpty()
                where o.OrderDate.Year == currentYear
                select new { o.OrderID, o.OrderDate }
            ).ToListAsync();

            return rows
                .Where(r => ISOWeek.GetWeekOfYear(r.OrderDate) == currentIsoWeek)
                .Select(r => new DistinctProductsThisWeekDto { OrderID = r.OrderID })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryDistinctProductsThisWeek.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders: order list with employee, customer, status, order total, and plain text notes.
    /// Inlines sub-queries qryOrderTotal and qrycboEmployees.
    /// </summary>
    public async Task<List<OrderListDto>> GetOrdersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildQryOrdersQuery(db)
                .OrderByDescending(o => o.OrderID)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrders.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_Customer: qryOrders filtered by customer and sorted by OrderDate descending.
    /// </summary>
    public async Task<List<OrderListDto>> GetMostRecentOrdersByCustomerAsync(int customerId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildQryOrdersQuery(db)
                .Where(o => o.CustomerID == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrders_MostRecent_Customer for customer {CustomerId}.", customerId);
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_Employee: qryOrders filtered by employee and sorted by OrderDate descending.
    /// </summary>
    public async Task<List<OrderListDto>> GetMostRecentOrdersByEmployeeAsync(int employeeId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildQryOrdersQuery(db)
                .Where(o => o.EmployeeID == employeeId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrders_MostRecent_Employee for employee {EmployeeId}.", employeeId);
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_ModifiedOn: qryOrders sorted by ModifiedOn descending.
    /// </summary>
    public async Task<List<OrderListDto>> GetMostRecentOrdersByModifiedOnAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildQryOrdersQuery(db)
                .OrderByDescending(o => o.ModifiedOn)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrders_MostRecent_ModifiedOn.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrders_MostRecent_OrderDate: qryOrders sorted by OrderDate descending.
    /// </summary>
    public async Task<List<OrderListDto>> GetMostRecentOrdersByOrderDateAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await BuildQryOrdersQuery(db)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrders_MostRecent_OrderDate.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryProductOrders: order details with order date and computed extended price.
    /// </summary>
    public async Task<List<ProductOrderDto>> GetProductOrdersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await (
                from o in db.Orders.AsNoTracking()
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID
                join p in db.Products.AsNoTracking() on od.ProductID equals p.ProductID
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
            _logger.LogError(ex, "Error executing qryProductOrders.");
            throw;
        }
    }

    public async Task<List<Orders>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Orders.AsNoTracking().OrderByDescending(o => o.OrderID).ToListAsync();
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
            _logger.LogError(ex, "Error retrieving order by id {OrderId}.", orderId);
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

    private static IQueryable<OrderListDto> BuildQryOrdersQuery(NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db)
    {
        var qryOrderTotal = db.OrderDetails
            .AsNoTracking()
            .GroupBy(od => od.OrderID)
            .Select(g => new
            {
                OrderID = g.Key,
                OrderTotal = g.Sum(od => od.Quantity * od.UnitPrice),
            });

        var qryCboEmployees = db.Employees
            .AsNoTracking()
            .Select(e => new
            {
                e.EmployeeID,
                e.FullNameFNLN,
                e.WindowsUserName,
            });

        return
            from o in db.Orders.AsNoTracking()
            join ot in qryOrderTotal on o.OrderID equals ot.OrderID into otGroup
            from ot in otGroup.DefaultIfEmpty()
            join e in qryCboEmployees on o.EmployeeID equals e.EmployeeID
            join c in db.Customers.AsNoTracking() on o.CustomerID equals c.CustomerID
            join os in db.OrderStatus.AsNoTracking() on o.StatusID equals os.StatusID
            select new OrderListDto
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
                FullNameFNLN = e.FullNameFNLN ?? string.Empty,
                CustomerName = c.CustomerName,
                StatusName = os.StatusName,
                OrderTotal = ot != null ? ot.OrderTotal : null,
                PlainTextNotes = o.Notes ?? string.Empty,
            };
    }
}
