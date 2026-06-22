using System.Net.Mail;

namespace NaturalezaViva.Domain.ValueObjects;

/// <summary>
/// Representa una dirección de correo electrónico válida en el dominio.
/// Value Object inmutable — si se construye, es válido.
/// </summary>
public class Email : IEquatable<Email>
{
    // ─── Propiedades ──────────────────────────────────────────────
    public string Value { get; }

    // ─── Constructor ──────────────────────────────────────────────
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío.", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!IsValid(normalized))
            throw new ArgumentException("El formato del email no es válido.", nameof(value));

        Value = normalized;
    }

    // ─── Validación ───────────────────────────────────────────────
    private static bool IsValid(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // ─── IEquatable<Email> ────────────────────────────────────────
    public bool Equals(Email? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as Email);
    public override int GetHashCode() => HashCode.Combine(Value);
    public override string ToString() => Value;

    public static bool operator ==(Email? left, Email? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }
    public static bool operator !=(Email? left, Email? right) => !(left == right);
}