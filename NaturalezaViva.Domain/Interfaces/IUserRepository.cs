using NaturalezaViva.Domain.Entities;
using NaturalezaViva.Domain.ValueObjects;

namespace NaturalezaViva.Domain.Interfaces;

public interface IUserRepository
{
    // ─── Consultas ────────────────────────────────────────────────
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDocumentAsync(Document document, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);

    // ─── Persistencia ─────────────────────────────────────────────
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}