using Api.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Api.Services;

public class RoleService(IRepository<RoleEntity> repository, ILogger<ProjectService> logger) : IRoleService
{
    private readonly ILogger<ProjectService> _logger = logger;
    public async Task<RoleEntity> CreateRoleAsync(RoleEntity role)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Skapar rollen: {role}", role.RoleName);
                
                var createdRole = await repository.AddAsync(role);
                await transaction.CommitAsync();
                
                _logger.LogInformation("Rollen: {role} har skapats.", role.RoleName);
                return createdRole;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation(ex, "Rollen: {role} kunde INTE skapas.", role.RoleName);
                throw;
            }
        }
    }

    public async Task<IEnumerable<RoleEntity>> GetAllRolesAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<RoleEntity?> GetRoleByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateRoleAsync(RoleEntity role)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Uppdaterar rollen: {role}", role.RoleName);
                
                var existingRole = await repository.GetByIdAsync(role.Id);

                if (existingRole == null)
                {
                    _logger.LogWarning("Rollen med roll ID: {RoleId} hittades inte.", role.Id);
                    throw new KeyNotFoundException($"Rollen med roll ID: {role.Id} hittades inte.");
                }
                
                existingRole.RoleName = role.RoleName;
                await repository.UpdateAsync(existingRole);
                await transaction.CommitAsync();
                
                _logger.LogInformation("Rollen: {role} har uppdaterats.", role.RoleName);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Rollen {RoleName} kunde INTE tas bort.", role.RoleName);
                throw;
            }
        }
    }

    public async Task DeleteRoleByIdAsync(int id)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Roll med RollId: {RoleId} tas bort.", id);
                
                await repository.DeleteAsync(id);
                await transaction.CommitAsync();
                _logger.LogInformation("Roll med RollId: {RoleId} har tagits bort.", id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Roll med ID: {RoleID} kunde INTE tas bort.", id);
                throw;
            }
        }
    }
}