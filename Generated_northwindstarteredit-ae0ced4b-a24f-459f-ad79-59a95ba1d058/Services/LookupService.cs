using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class CustomerLookupDto
{
    public int CustomerID { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}

public class OrderStatusLookupDto
{
    public int StatusID { get; set; }
    public string StatusName { get; set; } = string.Empty;
}

public sealed class LookupService : ILookupService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<LookupService> _logger;

    public LookupService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<LookupService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qrycboCustomers: customer lookup for dropdowns.
    /// </summary>
    public async Task<List<CustomerLookupDto>> GetCustomerLookupAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerName)
                .Select(c => new CustomerLookupDto
                {
                    CustomerID = c.CustomerID,
                    CustomerName = c.CustomerName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrycboCustomers.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrycboOrderStatus: order status lookup for dropdowns.
    /// </summary>
    public async Task<List<OrderStatusLookupDto>> GetOrderStatusLookupAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderStatus
                .AsNoTracking()
                .OrderBy(s => s.SortOrder)
                .Select(s => new OrderStatusLookupDto
                {
                    StatusID = s.StatusID,
                    StatusName = s.StatusName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrycboOrderStatus.");
            throw;
        }
    }
}
