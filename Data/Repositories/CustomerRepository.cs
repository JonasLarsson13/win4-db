using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository(DataContext context) : Repository<CustomerEntity>(context), ICustomerRepository
{
    private readonly DataContext _context = context;
    
    public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
    {
        if (customer.Contact != null)
        {
            _context.CustomerContacts.Add(customer.Contact);
            await _context.SaveChangesAsync();
            
            customer.ContactId = customer.Contact.Id;
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    
    public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Include(c => c.Contact)
            .ToListAsync();
    }


    public async Task<CustomerEntity?> GetCustomerWithContactAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Contact)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateCustomerAsync(CustomerEntity customer)
    {
        var existingCustomer = await _context.Customers
            .Include(c => c.Contact)
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        if (existingCustomer != null)
        {
            existingCustomer.CustomerName = customer.CustomerName;

            if (customer.Contact != null)
            {
                if (existingCustomer.Contact != null)
                {
                    existingCustomer.Contact.Name = customer.Contact.Name;
                    existingCustomer.Contact.Email = customer.Contact.Email;
                    existingCustomer.Contact.PhoneNumber = customer.Contact.PhoneNumber;
                }
                else
                {
                    var newContact = new CustomerContactEntity
                    {
                        Name = customer.Contact.Name,
                        Email = customer.Contact.Email,
                        PhoneNumber = customer.Contact.PhoneNumber
                    };

                    _context.CustomerContacts.Add(newContact);
                    await _context.SaveChangesAsync();

                    existingCustomer.ContactId = newContact.Id;
                    existingCustomer.Contact = newContact;
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Contact)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer != null)
        {
            if (customer.Contact != null)
            {
                _context.CustomerContacts.Remove(customer.Contact);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}