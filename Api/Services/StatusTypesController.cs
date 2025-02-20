using Api.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Api.Services;

public class StatusTypeService(IRepository<StatusTypeEntity> repository, ILogger<ProjectService> logger): IStatusTypeService
{
    private readonly ILogger<ProjectService> _logger = logger;
    public async Task<StatusTypeEntity> CreateStatusTypeAsync(StatusTypeEntity statusType)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Skapar status: {StatusTypeName}", statusType.StatusName);
                
                var createdStatus = await repository.AddAsync(statusType);
                await transaction.CommitAsync();
                
                _logger.LogInformation("Statusen {StatusTypeName} har skapats.", statusType.StatusName);
                return createdStatus;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Statusen: {StatusTypeName} kunde INTE skapas.", statusType.StatusName);
                throw;
            }
        }
    }

    public async Task<IEnumerable<StatusTypeEntity>> GetAllStatusTypesAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<StatusTypeEntity?> GetStatusTypeByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateStatusTypeAsync(StatusTypeEntity statusType)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Uppdaterar status: {StatusTypeName}", statusType.StatusName);
                
                var existingStatusType = await repository.GetByIdAsync(statusType.Id);

                if (existingStatusType == null)
                {
                    _logger.LogWarning("Status med ID: {StatusTypeId} kunde inte hittas.", statusType.Id);
                    throw new KeyNotFoundException($"Status med ID: {statusType.Id} kunde inte hittas.");
                }

                existingStatusType.StatusName = statusType.StatusName;

                await repository.UpdateAsync(existingStatusType);
                await transaction.CommitAsync();
                
                _logger.LogInformation("Statusen {StatusTypeName} har uppdaterats.", statusType.StatusName);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Status typen: {StatusTypeName} kunde INTE uppdateras.", statusType.StatusName);
                throw;
            }
        }
    }

    public async Task DeleteStatusTypeByIdAsync(int id)
    {
        using (var transaction = await repository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Tar bort status med ID: {StatusTypeId}", id);
                
                await repository.DeleteAsync(id);
                await transaction.CommitAsync();
                _logger.LogInformation("Statusen med ID: {StatusTypeId} har tagits bort.", id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Statusen med ID: {StatusTypeId} kunde INTE tas bort.", id);
                throw;
            }
        }
    }
}