// Domain/Events/UserCreatedEvent.cs
namespace NaturalezaViva.Domain.Events;

public record UserCreatedEvent(Guid UserId, string Email) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}