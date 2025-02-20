using Api.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Api.Services;

public class ProductService(IRepository<ProductEntity> repository, ILogger<ProjectService> logger) : IProductService
{
    private readonly ILogger<ProjectService> _logger = logger;
    public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Skapar produkt {ProductName}", product.ProductName);
                
                var createdProduct = await repository.AddAsync(product);
                await transaction.CommitAsync();
                _logger.LogInformation("Produkten {ProductName} har skapats.", product.ProductName);
                return createdProduct;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Kunde INTE skapa produkt {ProductName}.", product.ProductName);
                throw;
            }
        }
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<ProductEntity?> GetProductByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateProductAsync(ProductEntity product)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Uppdaterar produkt {ProductName}", product.ProductName);
                
                await repository.UpdateAsync(product);
                await transaction.CommitAsync();
                _logger.LogInformation("Produkten {ProductName} har uppdaterats.", product.ProductName);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Produkten {ProductName} kunde INTE uppdateras.", product.ProductName);
                throw;
            }
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Tar bort produkt med ID: {ProductId}.", id);
                
                await repository.DeleteAsync(id);
                await transaction.CommitAsync();
                _logger.LogInformation("Produkt med ID: {ProductId} har tagits bort.", id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Produkt med ID: {ProductId} kunde INTE tas bort.", id);
                throw;
            }
        }
    }
}