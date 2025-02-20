using Data.Entities;

namespace Api.Interfaces;

public interface IRoleService
{
    Task<RoleEntity> CreateRoleAsync(RoleEntity role);
    Task<IEnumerable<RoleEntity>> GetAllRolesAsync();
    Task<RoleEntity?> GetRoleByIdAsync(int id);
    Task UpdateRoleAsync(RoleEntity role);
    Task DeleteRoleByIdAsync(int id);
}