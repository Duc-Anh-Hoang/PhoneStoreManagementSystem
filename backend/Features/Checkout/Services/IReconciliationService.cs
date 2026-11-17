using PhoneStoreAPI.Features.Checkout.DTOs;

namespace PhoneStoreAPI.Features.Checkout.Services;

public interface IReconciliationService
{
    Task<ReconciliationReportDTO> ReconcileDailyTransactionsAsync(DateTime? date = null, CancellationToken cancellationToken = default);
}
