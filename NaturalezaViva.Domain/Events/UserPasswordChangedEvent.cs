// Domain/Events/UserPasswordChangedEvent.cs
using NaturalezaViva.Domain.Events;

public record UserPasswordChangedEvent(Guid UserId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}