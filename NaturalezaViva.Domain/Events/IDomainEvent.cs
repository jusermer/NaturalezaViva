// Domain/Events/IDomainEvent.cs
namespace NaturalezaViva.Domain.Events;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}