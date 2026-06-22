using NaturalezaViva.Domain.Enums;
using NaturalezaViva.Domain.Interfaces;
using NaturalezaViva.Domain.ValueObjects;
using NaturalezaViva.Domain.Events;

namespace NaturalezaViva.Domain.Entities;

public class User
{
    // ─── Propiedades ──────────────────────────────────────────────
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Document Document { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // ─── Eventos de dominio ───────────────────────────────────────
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // ─── Constructor privado — solo se crea desde el factory ──────
    private User() { }

    // ─── Factory Method ───────────────────────────────────────────
    public static User Create(
        string name,
        Email email,
        Document document,
        string plainPassword,
        IPasswordHasher hasher,
        UserRole role = UserRole.Cliente)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = ValidateName(name),
            Email = email ?? throw new ArgumentNullException(nameof(email)),
            Document = document ?? throw new ArgumentNullException(nameof(document)),
            PasswordHash = PasswordHash.Create(plainPassword, hasher),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        // Evento — el usuario fue creado
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email.Value));

        return user;
    }

    // ─── Comportamiento ───────────────────────────────────────────
    public void UpdateName(string name)
    {
        Name = ValidateName(name);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(Email email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDocument(Document document)
    {
        Document = document ?? throw new ArgumentNullException(nameof(document));
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string plainPassword, IPasswordHasher hasher)
    {
        PasswordHash = PasswordHash.Create(plainPassword, hasher);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordChangedEvent(Id));
    }

    public void SetRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
            throw new ArgumentException($"Rol inválido: {role}", nameof(role));

        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserActivatedEvent(Id));
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    // ─── Manejo de eventos ────────────────────────────────────────
    public void ClearDomainEvents() => _domainEvents.Clear();

    private void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    // ─── Validaciones privadas ────────────────────────────────────
    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));

        if (name.Trim().Length < 2)
            throw new ArgumentException("El nombre debe tener al menos 2 caracteres.", nameof(name));

        return name.Trim();
    }
}