using Data.Entities;

namespace Api.Interfaces;

public interface IStatusTypeService
{
    Task<StatusTypeEntity> CreateStatusTypeAsync(StatusTypeEntity statusType);
    Task<IEnumerable<StatusTypeEntity>> GetAllStatusTypesAsync();
    Task<StatusTypeEntity?> GetStatusTypeByIdAsync(int id);
    Task UpdateStatusTypeAsync(StatusTypeEntity statusType);
    Task DeleteStatusTypeByIdAsync(int id);
}