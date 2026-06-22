// Domain/Events/UserDeactivatedEvent.cs
namespace NaturalezaViva.Domain.Events; // ← debe coincidir exactamente

public record UserDeactivatedEvent(Guid UserId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}