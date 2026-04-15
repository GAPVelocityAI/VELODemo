using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class OrderTotalDto
{
    public int OrderID { get; set; }
    public decimal OrderTotal { get; set; }
}

public sealed class OrderDetailService : IOrderDetailService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<OrderDetailService> _logger;

    public OrderDetailService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<OrderDetailService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryOrderDetails: returns all order detail rows ordered by OrderDetailID.
    /// </summary>
    public async Task<List<OrderDetails>> GetOrderDetailsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails.AsNoTracking().OrderBy(od => od.OrderDetailID).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrderDetails.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryOrderTotal: sum of quantity * unit price grouped by order.
    /// </summary>
    public async Task<List<OrderTotalDto>> GetOrderTotalsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails
                .AsNoTracking()
                .GroupBy(od => od.OrderID)
                .Select(g => new OrderTotalDto
                {
                    OrderID = g.Key,
                    OrderTotal = g.Sum(od => od.UnitPrice * od.Quantity),
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrderTotal.");
            throw;
        }
    }

    public async Task<List<OrderDetails>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.OrderDetails.AsNoTracking().OrderBy(od => od.OrderDetailID).ToListAsync();
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
            _logger.LogError(ex, "Error retrieving order detail by id {OrderDetailId}.", orderDetailId);
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
