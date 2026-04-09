using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class ReportService : IReportService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<ReportService> _logger;

    public ReportService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<ReportService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryCustomerProductSummary.
    /// </summary>
    public async Task<List<CustomerProductSummaryDto>> GetCustomerProductSummaryAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await (
                from o in db.Orders
                join od in db.OrderDetails on o.OrderID equals od.OrderID
                join p in db.Products on od.ProductID equals p.ProductID
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
            _logger.LogError(ex, "Error running qryCustomerProductSummary.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryInvoice.
    /// </summary>
    public async Task<List<InvoiceDto>> GetInvoiceAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await (
                from c in db.Customers
                join o in db.Orders on c.CustomerID equals o.CustomerID
                join e in db.Employees on o.EmployeeID equals e.EmployeeID
                join od in db.OrderDetails on o.OrderID equals od.OrderID
                join p in db.Products on od.ProductID equals p.ProductID
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
                    SalesPerson = e.FullNameFNLN,
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
            _logger.LogError(ex, "Error running qryInvoice.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrySales_SalesRep.
    /// </summary>
    public async Task<List<SalesBySalesRepDto>> GetSalesBySalesRepAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var currentYear = DateTime.Today.Year;

            return await (
                from e in db.Employees
                join o in db.Orders on e.EmployeeID equals o.EmployeeID
                join od in db.OrderDetails on o.OrderID equals od.OrderID
                where o.OrderDate.Year == currentYear
                group new { o, od } by e.FullNameFNLN
                into g
                select new SalesBySalesRepDto
                {
                    FullNameFNLN = g.Key,
                    OrderTotal = g.Sum(x => x.od.Quantity * x.od.UnitPrice),
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrySales_SalesRep.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrySales_YTD.
    /// </summary>
    public async Task<decimal> GetSalesYtdAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var currentYear = DateTime.Today.Year;

            return await (
                from o in db.Orders
                join od in db.OrderDetails on o.OrderID equals od.OrderID
                where o.OrderDate.Year == currentYear
                select (decimal?)od.Quantity * od.UnitPrice)
                .SumAsync() ?? 0m;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrySales_YTD.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptEmployeeEmailList.
    /// </summary>
    public async Task<List<EmployeeEmailListDto>> GetEmployeeEmailListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees
                .OrderBy(e => e.FullNameLNFN)
                .Select(e => new EmployeeEmailListDto
                {
                    EmployeeID = e.EmployeeID,
                    FullNameFNLN = e.FullNameFNLN,
                    EmailAddress = e.EmailAddress,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryrptEmployeeEmailList.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptEmployeePhoneList.
    /// </summary>
    public async Task<List<EmployeePhoneListDto>> GetEmployeePhoneListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees
                .OrderBy(e => e.FullNameLNFN)
                .Select(e => new EmployeePhoneListDto
                {
                    EmployeeID = e.EmployeeID,
                    FullNameFNLN = e.FullNameFNLN,
                    PrimaryPhone = e.PrimaryPhone,
                    SecondaryPhone = e.SecondaryPhone,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryrptEmployeePhoneList.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptSalesByEmployee and inlines sub-query qryOrderTotal.
    /// </summary>
    public async Task<List<SalesByEmployeeReportDto>> GetReportSalesByEmployeeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var endDateExclusive = endDate.Date.AddDays(1);

            var qryOrderTotal = db.OrderDetails
                .GroupBy(od => od.OrderID)
                .Select(g => new
                {
                    OrderID = g.Key,
                    OrderTotal = g.Sum(x => x.Quantity * x.UnitPrice),
                });

            var rows = await (
                from ot in qryOrderTotal
                join o in db.Orders on ot.OrderID equals o.OrderID
                join e in db.Employees on o.EmployeeID equals e.EmployeeID
                where o.OrderDate >= startDate && o.OrderDate < endDateExclusive
                group new { o, e, ot } by new { o.EmployeeID, e.FullNameFNLN, o.OrderDate.Year, o.OrderDate.Month }
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

            return rows.Select(r =>
            {
                var monthDate = new DateTime(r.Year, r.Month, 1);
                return new SalesByEmployeeReportDto
                {
                    EmployeeID = r.EmployeeID,
                    FullNameFNLN = r.FullNameFNLN,
                    OrderTotal = r.OrderTotal,
                    MonthYear = monthDate.ToString("MMM-yyyy"),
                    MonthYearSort = monthDate.ToString("yyyy-MM"),
                };
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryrptSalesByEmployee.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptSalesByProduct.
    /// </summary>
    public async Task<List<SalesByProductReportDto>> GetReportSalesByProductAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var endDateExclusive = endDate.Date.AddDays(1);

            var rows = await (
                from o in db.Orders
                join od in db.OrderDetails on o.OrderID equals od.OrderID
                join p in db.Products on od.ProductID equals p.ProductID
                where o.OrderDate >= startDate && o.OrderDate < endDateExclusive
                group new { o, od, p } by new { p.ProductName, p.ProductID, o.OrderDate.Year, o.OrderDate.Month }
                into g
                select new
                {
                    g.Key.ProductName,
                    g.Key.ProductID,
                    g.Key.Year,
                    g.Key.Month,
                    OrderTotal = g.Sum(x => x.od.Quantity * x.od.UnitPrice),
                })
                .ToListAsync();

            return rows.Select(r =>
            {
                var monthDate = new DateTime(r.Year, r.Month, 1);
                var quarter = ((r.Month - 1) / 3) + 1;

                return new SalesByProductReportDto
                {
                    ProductName = r.ProductName,
                    ProductID = r.ProductID,
                    OrderTotal = r.OrderTotal,
                    MonthYear = monthDate.ToString("MMM-yyyy"),
                    MonthYearSort = monthDate.ToString("yyyy-MM"),
                    QuarterYear = $"{quarter}-{r.Year}",
                };
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryrptSalesByProduct.");
            throw;
        }
    }

    /// <summary>
    /// Alias for Access report query qryrptSalesByEmployee.
    /// </summary>
    public Task<List<SalesByEmployeeReportDto>> GetRptSalesByEmployeeAsync(DateTime startDate, DateTime endDate)
        => GetReportSalesByEmployeeAsync(startDate, endDate);

    /// <summary>
    /// Alias for Access report query qryrptSalesByProduct.
    /// </summary>
    public Task<List<SalesByProductReportDto>> GetRptSalesByProductAsync(DateTime startDate, DateTime endDate)
        => GetReportSalesByProductAsync(startDate, endDate);
}

