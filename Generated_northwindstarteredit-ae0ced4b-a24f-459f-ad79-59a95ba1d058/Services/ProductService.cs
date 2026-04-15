using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public sealed class ProductService : IProductService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<ProductService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryProductDetail: returns all product columns ordered by ProductCode.
    /// </summary>
    public async Task<List<Products>> GetProductDetailAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.AsNoTracking().OrderBy(p => p.ProductCode).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryProductDetail.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrycboProducts: product lookup ordered by ProductName.
    /// </summary>
    public async Task<List<Products>> GetProductLookupAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.AsNoTracking().OrderBy(p => p.ProductName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrycboProducts.");
            throw;
        }
    }

    public async Task<List<Products>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Products.AsNoTracking().OrderBy(p => p.ProductCode).ToListAsync();
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
            _logger.LogError(ex, "Error retrieving product by id {ProductId}.", productId);
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
