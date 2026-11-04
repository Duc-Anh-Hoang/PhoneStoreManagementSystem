using PhoneStoreAPI.Features.Promotions.DTOs;

namespace PhoneStoreAPI.Features.Promotions;

public interface IPromotionService
{
    Task<List<PromotionDto>> GetAllPromotionsAsync();
    Task<List<ActivePromotionDto>> GetActivePromotionsAsync();
    Task<PromotionDto> GetPromotionByIdAsync(string id);
    Task<PromotionDto> CreatePromotionAsync(CreatePromotionRequest request);
    Task<PromotionDto> UpdatePromotionAsync(string id, UpdatePromotionRequest request);
    Task<bool> UpdateStatusAsync(string id, bool tamDung);
    Task DeletePromotionAsync(string id);
}
