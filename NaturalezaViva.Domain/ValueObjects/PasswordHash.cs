using NaturalezaViva.Domain.Interfaces;

namespace NaturalezaViva.Domain.ValueObjects;
public class PasswordHash : IEquatable<PasswordHash>
{
    private const int MinPasswordLength = 8;
    public string Value { get; }

    // Constructor privado — solo se crea desde los factory methods
    private PasswordHash(string value)
    {
        Value = value;
    }

    // ─── Factory Methods ──────────────────────────────────────────

    /// <summary>
    /// Usalo al registrar o cambiar contraseña.
    /// El hashing lo hace la infraestructura via IPasswordHasher.
    /// </summary>
    public static PasswordHash Create(string plainPassword, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentException("La contraseña no puede estar vacía.", nameof(plainPassword));

        if (plainPassword.Length < MinPasswordLength)
            throw new ArgumentException($"La contraseña debe tener al menos {MinPasswordLength} caracteres.", nameof(plainPassword));

        var hash = hasher.Hash(plainPassword);
        return new PasswordHash(hash);
    }

    /// <summary>
    /// Solo para reconstruir desde base de datos.
    /// </summary>
    public static PasswordHash FromHash(string hashedValue)
    {
        if (string.IsNullOrWhiteSpace(hashedValue))
            throw new ArgumentException("El hash no puede estar vacío.", nameof(hashedValue));

        return new PasswordHash(hashedValue);
    }

    // ─── Verificación ─────────────────────────────────────────────
    public bool Verify(string plainPassword, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(plainPassword)) return false;
        return hasher.Verify(plainPassword, Value);
    }

    // ─── IEquatable<PasswordHash> ─────────────────────────────────
    public bool Equals(PasswordHash? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as PasswordHash);
    public override int GetHashCode() => HashCode.Combine(Value);
    public override string ToString() => "********";

    public static bool operator ==(PasswordHash? left, PasswordHash? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }
    public static bool operator !=(PasswordHash? left, PasswordHash? right) => !(left == right);
}