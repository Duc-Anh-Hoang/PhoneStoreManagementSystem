using PhoneStoreAPI.Features.Sales.Returns.DTOs;

namespace PhoneStoreAPI.Features.Sales.Returns;

public interface IReturnsService
{
    Task<TraHangDto> CreateTraHangAsync(CreateTraHangRequest request);
    Task<(List<TraHangDto> Items, int Total)> GetTraHangsAsync(DateOnly? tuNgay, DateOnly? denNgay, int page, int pageSize);
}
