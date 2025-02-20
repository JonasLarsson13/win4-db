using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        return employee != null ? Ok(employee) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var employee = new EmployeeEntity
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            RoleId = employeeDto.RoleId
        };

        var createdEmployee = await employeeService.CreateEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeEntity employee)
    {
        if (id != employee.Id) return BadRequest("ID mismatch.");
        
        await employeeService.UpdateEmployeeAsync(employee);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
    
    [HttpGet("project-managers")]
    public async Task<IActionResult> GetProjectManagers()
    {
        var projectManagers = await employeeService.GetProjectManagersAsync();
        return Ok(projectManagers);
    }
}