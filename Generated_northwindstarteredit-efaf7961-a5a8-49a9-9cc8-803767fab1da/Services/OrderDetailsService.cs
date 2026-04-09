using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class OrderDetailsService : IOrderDetailsService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<OrderDetailsService> _logger;

    public OrderDetailsService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<OrderDetailsService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryOrderDetails (OrderDetails.* ordered by OrderDetailID).
    /// </summary>
    public async Task<List<OrderDetails>> GetOrderDetailsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails.OrderBy(od => od.OrderDetailID).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrderDetails.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrderTotal (grouped Sum(Quantity * UnitPrice) by OrderID).
    /// </summary>
    public async Task<List<OrderTotalDto>> GetOrderTotalAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await db.OrderDetails
                .GroupBy(od => od.OrderID)
                .Select(g => new OrderTotalDto
                {
                    OrderID = g.Key,
                    OrderTotal = g.Sum(x => x.UnitPrice * x.Quantity),
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryOrderTotal.");
            throw;
        }
    }

    public async Task<List<OrderDetails>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails.OrderBy(od => od.OrderDetailID).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all order details.");
            throw;
        }
    }

    public async Task<OrderDetails?> GetByIdAsync(int orderDetailId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailID == orderDetailId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order detail {OrderDetailId}.", orderDetailId);
            throw;
        }
    }

    public async Task<OrderDetails> CreateAsync(OrderDetails orderDetail)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.OrderDetails.Add(orderDetail);
            await db.SaveChangesAsync();
            return orderDetail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order detail.");
            throw;
        }
    }

    public async Task<OrderDetails> UpdateAsync(OrderDetails orderDetail)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.OrderDetails.Update(orderDetail);
            await db.SaveChangesAsync();
            return orderDetail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order detail {OrderDetailId}.", orderDetail.OrderDetailID);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int orderDetailId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails.Where(od => od.OrderDetailID == orderDetailId).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting order detail {OrderDetailId}.", orderDetailId);
            throw;
        }
    }
}
