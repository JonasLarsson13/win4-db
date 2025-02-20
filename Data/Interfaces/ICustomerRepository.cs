using Data.Entities;
using Data.Repositories;

namespace Data.Interfaces;

public interface ICustomerRepository : IRepository<CustomerEntity>
{
    Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer);
    Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync();
    Task<CustomerEntity?> GetCustomerWithContactAsync(int id);
    Task UpdateCustomerAsync(CustomerEntity customer);
    Task DeleteCustomerAsync(int id);
}