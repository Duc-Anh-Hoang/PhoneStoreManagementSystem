using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Promotions;

public interface IPromotionService
{
    Task<List<KhuyenMai>> GetAllPromotionsAsync();
    Task<List<KhuyenMai>> GetActivePromotionsAsync();
    Task<KhuyenMai?> GetPromotionByIdAsync(string id);
    Task<KhuyenMai> CreatePromotionAsync(KhuyenMai km);
    Task<bool> UpdateStatusAsync(string id, bool tamDung);
    Task<bool> DeletePromotionAsync(string id);
}
