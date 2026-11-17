namespace PhoneStoreAPI.Features.Checkout.Services;

public record StateTransitionResult(bool Success, string? ErrorMessage = null);

public interface IOrderStateMachine
{
    bool CanTransition(string fromStatus, string toStatus);
    Task<StateTransitionResult> TransitionAsync(long orderId, string expectedCurrentStatus, string targetStatus, string? reason = null, CancellationToken cancellationToken = default);
}
