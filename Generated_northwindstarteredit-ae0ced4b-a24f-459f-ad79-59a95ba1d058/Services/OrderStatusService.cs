using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class OrderWithStatusDto
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
    public string StatusName { get; set; } = string.Empty;
}

public sealed class OrderStatusService : IOrderStatusService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<OrderStatusService> _logger;

    public OrderStatusService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<OrderStatusService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryOrder: joins Orders and OrderStatus and includes StatusName.
    /// </summary>
    public async Task<List<OrderWithStatusDto>> GetOrdersWithStatusAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await (
                from o in db.Orders.AsNoTracking()
                join os in db.OrderStatus.AsNoTracking() on o.StatusID equals os.StatusID
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
                    StatusName = os.StatusName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryOrder.");
            throw;
        }
    }
}
