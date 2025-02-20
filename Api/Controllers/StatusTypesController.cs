using Api.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusTypesController(IStatusTypeService statusTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllStatusTypes()
    {
        var statusTypes = await statusTypeService.GetAllStatusTypesAsync();
        return Ok(statusTypes);
    }

    [HttpGet("{id:int}", Name = "GetStatusTypeById")]
    public async Task<IActionResult> GetStatusTypeByIdAsync(int id)
    {
        var statusType = await statusTypeService.GetStatusTypeByIdAsync(id);
        if (statusType == null)
        {
            return NotFound($"StatusType with ID {id} not found.");
        }
        return Ok(statusType);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStatusTypeAsync(StatusTypeEntity statusType)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdStatusType = await statusTypeService.CreateStatusTypeAsync(statusType);
        return CreatedAtAction("GetStatusTypeById", new { id = createdStatusType.Id }, createdStatusType);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStatusType(int id, StatusTypeEntity statusType)
    {
        if (id != statusType.Id)
        {
            return BadRequest("ID in URL does not match ID in body.");
        }

        var existingStatusType = await statusTypeService.GetStatusTypeByIdAsync(id);
        if (existingStatusType == null)
        {
            return NotFound($"StatusType with ID {id} not found.");
        }

        await statusTypeService.UpdateStatusTypeAsync(statusType);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStatusType(int id)
    {
        var existingStatusType = await statusTypeService.GetStatusTypeByIdAsync(id);
        if (existingStatusType == null)
        {
            return NotFound($"StatusType with ID {id} not found.");
        }

        await statusTypeService.DeleteStatusTypeByIdAsync(id);
        return NoContent();
    }
}