// Domain/Events/UserActivatedEvent.cs
using NaturalezaViva.Domain.Events;

public record UserActivatedEvent(Guid UserId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}