using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IProductService
{
    Task<List<Products>> GetProductDetailAsync();
    Task<List<ProductSalesSummaryDto>> GetProductsAsync();
    Task<List<CboProductDto>> GetCboProductsAsync();
    Task<List<Products>> GetAllAsync();
    Task<Products?> GetByIdAsync(int productId);
    Task<Products> CreateAsync(Products product);
    Task<Products> UpdateAsync(Products product);
    Task<int> DeleteAsync(int productId);
}
