using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class LookupService : ILookupService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<LookupService> _logger;

    public LookupService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<LookupService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qrycboCustomers.
    /// </summary>
    public async Task<List<CboCustomerDto>> GetCboCustomersAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers
                .OrderBy(c => c.CustomerName)
                .Select(c => new CboCustomerDto
                {
                    CustomerID = c.CustomerID,
                    CustomerName = c.CustomerName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrycboCustomers.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrycboOrderStatus.
    /// </summary>
    public async Task<List<CboOrderStatusDto>> GetCboOrderStatusAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderStatus
                .OrderBy(s => s.SortOrder)
                .Select(s => new CboOrderStatusDto
                {
                    StatusID = s.StatusID,
                    StatusName = s.StatusName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrycboOrderStatus.");
            throw;
        }
    }
}
