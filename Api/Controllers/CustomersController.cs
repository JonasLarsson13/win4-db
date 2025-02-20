using Api.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CustomerEntity customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCustomer = await customerService.CreateCustomerAsync(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await customerService.GetCustomerByIdAsync(id);
        return customer != null ? Ok(customer) : NotFound($"Customer with ID {id} not found.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, CustomerEntity customer)
    {
        if (id != customer.Id)
        {
            return BadRequest("ID mismatch.");
        }

        await customerService.UpdateCustomerAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}