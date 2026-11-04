using PhoneStoreAPI.Features.SystemSettings.DTOs;

namespace PhoneStoreAPI.Features.SystemSettings;

public interface ISystemSettingService
{
    Task<List<ThamSoDto>> GetAllAsync();
    Task<ThamSoDto> GetByMaAsync(string ma);
    Task<ThamSoDto> UpdateAsync(string ma, UpdateThamSoRequest request);
}
