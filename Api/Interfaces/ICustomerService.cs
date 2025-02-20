using Data.Entities;

namespace Api.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync();
    Task<CustomerEntity?> GetCustomerByIdAsync(int id);
    Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer);
    Task UpdateCustomerAsync(CustomerEntity customer);
    Task DeleteCustomerAsync(int id);
}