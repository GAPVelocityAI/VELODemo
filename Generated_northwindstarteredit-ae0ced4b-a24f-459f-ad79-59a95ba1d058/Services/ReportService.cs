using System.Globalization;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class InvoiceDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
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
    public string SalesPerson { get; set; } = string.Empty;
    public int ProductID { get; set; }
    public short Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
}

public class ProductSummaryDto
{
    public int ProductID { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public int QtySold { get; set; }
    public int SoldThisWeek { get; set; }
}

public class SalesBySalesRepDto
{
    public decimal OrderTotal { get; set; }
    public string FullNameFNLN { get; set; } = string.Empty;
}

public class SalesYtdDto
{
    public decimal OrderTotal { get; set; }
}

public class SalesByEmployeeReportDto
{
    public int EmployeeID { get; set; }
    public decimal OrderTotal { get; set; }
    public string FullNameFNLN { get; set; } = string.Empty;
    public string MonthYear { get; set; } = string.Empty;
    public string MonthYearSort { get; set; } = string.Empty;
}

public class SalesByProductReportDto
{
    public string ProductName { get; set; } = string.Empty;
    public int ProductID { get; set; }
    public decimal OrderTotal { get; set; }
    public string MonthYear { get; set; } = string.Empty;
    public string MonthYearSort { get; set; } = string.Empty;
    public string QuarterYear { get; set; } = string.Empty;
}

public sealed class ReportService : IReportService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<ReportService> _logger;

    public ReportService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<ReportService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryInvoice: invoice projection across customers, orders, order details, products, and employees.
    /// </summary>
    public async Task<List<InvoiceDto>> GetInvoiceAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await (
                from c in db.Customers.AsNoTracking()
                join o in db.Orders.AsNoTracking() on c.CustomerID equals o.CustomerID
                join e in db.Employees.AsNoTracking() on o.EmployeeID equals e.EmployeeID
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID
                join p in db.Products.AsNoTracking() on od.ProductID equals p.ProductID
                select new InvoiceDto
                {
                    CustomerName = c.CustomerName,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    Zip = c.Zip,
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
                    SalesPerson = e.FullNameFNLN ?? string.Empty,
                    ProductID = od.ProductID,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryInvoice.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryProducts: product sales summary with QtySold and SoldThisWeek.
    /// Inlines sub-query qryDistinctProductsThisWeek.
    /// </summary>
    public async Task<List<ProductSummaryDto>> GetProductsSummaryAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var today = DateTime.Today;
            var currentYear = today.Year;
            var currentIsoWeek = ISOWeek.GetWeekOfYear(today);

            var currentWeekOrderIds = (await db.Orders
                    .AsNoTracking()
                    .Where(o => o.OrderDate.Year == currentYear)
                    .Select(o => new { o.OrderID, o.OrderDate })
                    .ToListAsync())
                .Where(o => ISOWeek.GetWeekOfYear(o.OrderDate) == currentIsoWeek)
                .Select(o => o.OrderID)
                .ToList();

            return await (
                from p in db.Products.AsNoTracking()
                join od in db.OrderDetails.AsNoTracking() on p.ProductID equals od.ProductID into odGroup
                from od in odGroup.DefaultIfEmpty()
                group od by new
                {
                    p.ProductID,
                    p.ProductCode,
                    p.ProductName,
                    p.ProductDescription,
                    p.UnitPrice,
                }
                into g
                orderby g.Key.ProductCode
                select new ProductSummaryDto
                {
                    ProductID = g.Key.ProductID,
                    ProductCode = g.Key.ProductCode,
                    ProductName = g.Key.ProductName,
                    ProductDescription = g.Key.ProductDescription,
                    UnitPrice = g.Key.UnitPrice,
                    QtySold = g.Count(od => od != null),
                    SoldThisWeek = g.Count(od => od != null && currentWeekOrderIds.Contains(od.OrderID)),
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryProducts.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrySales_SalesRep: current-year sales totals grouped by employee FullNameFNLN.
    /// </summary>
    public async Task<List<SalesBySalesRepDto>> GetSalesBySalesRepAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var currentYear = DateTime.Today.Year;

            return await (
                from e in db.Employees.AsNoTracking()
                join o in db.Orders.AsNoTracking() on e.EmployeeID equals o.EmployeeID
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID
                where o.OrderDate.Year == currentYear
                group new { e, od } by e.FullNameFNLN
                into g
                select new SalesBySalesRepDto
                {
                    FullNameFNLN = g.Key ?? string.Empty,
                    OrderTotal = g.Sum(x => x.od.Quantity * x.od.UnitPrice),
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrySales_SalesRep.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrySales_YTD: current-year total sales amount.
    /// </summary>
    public async Task<List<SalesYtdDto>> GetSalesYtdAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var currentYear = DateTime.Today.Year;

            var total = await (
                from o in db.Orders.AsNoTracking()
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID
                where o.OrderDate.Year == currentYear
                select od.Quantity * od.UnitPrice
            ).DefaultIfEmpty(0m).SumAsync();

            return [new SalesYtdDto { OrderTotal = total }];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrySales_YTD.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptSalesByEmployee: sales totals by employee and month-year for a date range.
    /// Inlines sub-query qryOrderTotal.
    /// </summary>
    public async Task<List<SalesByEmployeeReportDto>> GetSalesByEmployeeReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var inclusiveEndDate = endDate.AddDays(1);

            var qryOrderTotal = db.OrderDetails
                .AsNoTracking()
                .GroupBy(od => od.OrderID)
                .Select(g => new
                {
                    OrderID = g.Key,
                    OrderTotal = g.Sum(od => od.Quantity * od.UnitPrice),
                });

            var rawRows = await (
                from ot in qryOrderTotal
                join o in db.Orders.AsNoTracking() on ot.OrderID equals o.OrderID
                join e in db.Employees.AsNoTracking() on o.EmployeeID equals e.EmployeeID
                where o.OrderDate >= startDate && o.OrderDate <= inclusiveEndDate
                group new { o, e, ot } by new
                {
                    o.EmployeeID,
                    e.FullNameFNLN,
                    o.OrderDate.Year,
                    o.OrderDate.Month,
                }
                into g
                select new
                {
                    g.Key.EmployeeID,
                    g.Key.FullNameFNLN,
                    g.Key.Year,
                    g.Key.Month,
                    OrderTotal = g.Sum(x => x.ot.OrderTotal),
                })
                .ToListAsync();

            return rawRows
                .Select(x =>
                {
                    var monthStart = new DateTime(x.Year, x.Month, 1);
                    return new SalesByEmployeeReportDto
                    {
                        EmployeeID = x.EmployeeID,
                        OrderTotal = x.OrderTotal,
                        FullNameFNLN = x.FullNameFNLN ?? string.Empty,
                        MonthYear = monthStart.ToString("MMM-yyyy", CultureInfo.InvariantCulture),
                        MonthYearSort = monthStart.ToString("yyyy-MM", CultureInfo.InvariantCulture),
                    };
                })
                .OrderBy(x => x.MonthYearSort)
                .ThenBy(x => x.FullNameFNLN)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryrptSalesByEmployee.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptSalesByProduct: sales totals by product with month-year and quarter-year fields.
    /// </summary>
    public async Task<List<SalesByProductReportDto>> GetSalesByProductReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var inclusiveEndDate = endDate.AddDays(1);

            var rawRows = await (
                from o in db.Orders.AsNoTracking()
                join od in db.OrderDetails.AsNoTracking() on o.OrderID equals od.OrderID
                join p in db.Products.AsNoTracking() on od.ProductID equals p.ProductID
                where o.OrderDate >= startDate && o.OrderDate <= inclusiveEndDate
                group new { o, od, p } by new
                {
                    p.ProductName,
                    p.ProductID,
                    o.OrderDate.Year,
                    o.OrderDate.Month,
                    Quarter = ((o.OrderDate.Month - 1) / 3) + 1,
                }
                into g
                select new
                {
                    g.Key.ProductName,
                    g.Key.ProductID,
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Quarter,
                    OrderTotal = g.Sum(x => x.od.Quantity * x.od.UnitPrice),
                })
                .ToListAsync();

            return rawRows
                .Select(x =>
                {
                    var monthStart = new DateTime(x.Year, x.Month, 1);
                    return new SalesByProductReportDto
                    {
                        ProductName = x.ProductName,
                        ProductID = x.ProductID,
                        OrderTotal = x.OrderTotal,
                        MonthYear = monthStart.ToString("MMM-yyyy", CultureInfo.InvariantCulture),
                        MonthYearSort = monthStart.ToString("yyyy-MM", CultureInfo.InvariantCulture),
                        QuarterYear = $"{x.Quarter}-{x.Year}",
                    };
                })
                .OrderBy(x => x.MonthYearSort)
                .ThenBy(x => x.ProductName)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryrptSalesByProduct.");
            throw;
        }
    }
}
