using Api.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        var roles = await roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id:int}", Name = "GetRoleById")]
    public async Task<IActionResult> GetRoleByIdAsync(int id)
    {
        var role = await roleService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound($"Role with ID {id} not found.");
        }
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync(RoleEntity role)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var createdRole = await roleService.CreateRoleAsync(role);
        return CreatedAtAction("GetRoleById", new { id = createdRole.Id }, createdRole);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRoleAsync(int id, RoleEntity role)
    {
        if (id != role.Id)
        {
            return BadRequest("ID in URL does not match ID in body.");
        }
        
        var existingRole = await roleService.GetRoleByIdAsync(id);
        if (existingRole == null)
        {
            return NotFound($"Role with ID {id} not found.");
        }
        
        await roleService.UpdateRoleAsync(role);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(int id)
    {
        var existingRole = await roleService.GetRoleByIdAsync(id);
        if (existingRole == null)
        {
            return NotFound($"Role with ID {id} not found.");
        }
        
        await roleService.DeleteRoleByIdAsync(id);
        return NoContent();
    }
}