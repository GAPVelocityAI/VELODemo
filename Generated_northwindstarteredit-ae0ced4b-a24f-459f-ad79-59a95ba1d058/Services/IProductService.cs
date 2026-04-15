using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IProductService
{
    Task<List<Products>> GetProductDetailAsync();
    Task<List<Products>> GetProductLookupAsync();
    Task<List<Products>> GetAllAsync();
    Task<Products?> GetByIdAsync(int productId);
    Task<Products> CreateAsync(Products product);
    Task<Products> UpdateAsync(Products product);
    Task<int> DeleteAsync(int productId);
}
