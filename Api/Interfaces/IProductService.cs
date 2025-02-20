using Data.Entities;

namespace Api.Interfaces;

public interface IProductService
{
    Task<ProductEntity> CreateProductAsync(ProductEntity product);
    Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
    Task<ProductEntity?> GetProductByIdAsync(int id);
    Task UpdateProductAsync(ProductEntity product);
    Task DeleteProductAsync(int id);
}