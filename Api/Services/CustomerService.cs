using Api.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Api.Services;

public class CustomerService(ICustomerRepository customerRepository, ILogger<ProjectService> logger) : ICustomerService
{
    private readonly ILogger<ProjectService> _logger = logger;
    public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
    {
        using (var transaction = await customerRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Skapar kund: {CustomerName}", customer.CustomerName);
                
                if (customer.Contact == null) return await customerRepository.CreateCustomerAsync(customer);
                var contact = new CustomerContactEntity
                {
                    Name = customer.Contact.Name,
                    Email = customer.Contact.Email,
                    PhoneNumber = customer.Contact.PhoneNumber
                };

                customer.Contact = contact;

                var createdCustomer = await customerRepository.CreateCustomerAsync(customer);
                await transaction.CommitAsync();
                _logger.LogInformation("Kund {CustomerName} har skapats", customer.CustomerName);
                return createdCustomer;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Kunden: {CustomerName} kunde INTE skapas.", customer.CustomerName);
                throw;
            }
        }
    }
    
    public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
    {
        return await customerRepository.GetAllCustomersAsync();
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int id)
    {
        return await customerRepository.GetCustomerWithContactAsync(id);
    }

    public async Task UpdateCustomerAsync(CustomerEntity customer)
    {
        using (var transaction = await customerRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Uppdaterar kund: {CustomerName}", customer.CustomerName);
                
                await customerRepository.UpdateCustomerAsync(customer);
                await transaction.CommitAsync();
                _logger.LogInformation("Kunden {CustomerName} har uppdaterats.", customer.CustomerName);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Kunden: {CustomerName} kunde INTE uppdateras.", customer.CustomerName);
                throw;
            }
        }
    }

    public async Task DeleteCustomerAsync(int id)
    {
        using (var transaction = await customerRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Kund med ID: {CustomerId} tas bort.", id);
                
                await customerRepository.DeleteCustomerAsync(id);
                await transaction.CommitAsync();
                _logger.LogInformation("Kund med ID: {CustomerId} har tagits bort.", id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Kund med ID: {CustomerId} kunde INTE tas bort.", id);
                throw;
            }
        }
    }
}