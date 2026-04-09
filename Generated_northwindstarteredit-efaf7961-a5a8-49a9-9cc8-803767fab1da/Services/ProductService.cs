using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class ProductService : IProductService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<ProductService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryProductDetail (Products.* ordered by ProductCode).
    /// </summary>
    public async Task<List<Products>> GetProductDetailAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.OrderBy(p => p.ProductCode).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryProductDetail.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryProducts, including the qryDistinctProductsThisWeek sub-query logic.
    /// </summary>
    public async Task<List<ProductSalesSummaryDto>> GetProductsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            var today = DateTime.Today;
            var isoYear = ISOWeek.GetYear(today);
            var isoWeek = ISOWeek.GetWeekOfYear(today);
            var weekStart = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday);
            var weekEndExclusive = weekStart.AddDays(7);

            var distinctOrderIdsThisWeek = db.Orders
                .Where(o => o.OrderDate >= weekStart && o.OrderDate < weekEndExclusive)
                .Select(o => o.OrderID)
                .Distinct();

            return await db.Products
                .GroupJoin(
                    db.OrderDetails,
                    p => p.ProductID,
                    od => od.ProductID,
                    (p, orderDetails) => new { p, orderDetails })
                .Select(x => new ProductSalesSummaryDto
                {
                    ProductID = x.p.ProductID,
                    ProductCode = x.p.ProductCode,
                    ProductName = x.p.ProductName,
                    ProductDescription = x.p.ProductDescription,
                    UnitPrice = x.p.UnitPrice,
                    QtySold = x.orderDetails.Count(),
                    SoldThisWeek = x.orderDetails.Count(od => distinctOrderIdsThisWeek.Contains(od.OrderID)),
                })
                .OrderBy(x => x.ProductCode)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryProducts.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrycboProducts.
    /// </summary>
    public async Task<List<CboProductDto>> GetCboProductsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await db.Products
                .OrderBy(p => p.ProductName)
                .Select(p => new CboProductDto
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrycboProducts.");
            throw;
        }
    }

    public async Task<List<Products>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.OrderBy(p => p.ProductCode).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products.");
            throw;
        }
    }

    public async Task<Products?> GetByIdAsync(int productId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product {ProductId}.", productId);
            throw;
        }
    }

    public async Task<Products> CreateAsync(Products product)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product.");
            throw;
        }
    }

    public async Task<Products> UpdateAsync(Products product)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Products.Update(product);
            await db.SaveChangesAsync();
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}.", product.ProductID);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int productId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.Where(p => p.ProductID == productId).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId}.", productId);
            throw;
        }
    }
}
