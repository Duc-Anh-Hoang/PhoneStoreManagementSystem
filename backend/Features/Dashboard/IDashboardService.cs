namespace PhoneStoreAPI.Features.Dashboard;

using PhoneStoreAPI.Features.Dashboard.DTOs;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
    Task<List<ChannelRevenueDto>> GetRevenueByChannelAsync(DateTime? tuNgay, DateTime? denNgay);
    Task<List<CategoryRevenueDto>> GetRevenueByCategoryAsync(DateTime? tuNgay, DateTime? denNgay);
}
