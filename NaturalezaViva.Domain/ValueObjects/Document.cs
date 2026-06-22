using System.Text.RegularExpressions;

namespace NaturalezaViva.Domain.ValueObjects;

public class Document : IEquatable<Document>
{
    // ─── Propiedades ──────────────────────────────────────────────
    public DocumentType Type { get; }
    public string Number { get; }

    // ─── Constructor público con validaciones ─────────────────────
    public Document(DocumentType type, string number)
    {
        if (!Enum.IsDefined(typeof(DocumentType), type))
            throw new ArgumentException("Tipo de documento inválido.", nameof(type));

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("El número no puede estar vacío.", nameof(number));

        ValidateByType(type, number.Trim());

        Type = type;
        Number = number.Trim();
    }

    // ─── Validación por tipo ──────────────────────────────────────
    private static void ValidateByType(DocumentType type, string number)
    {
        switch (type)
        {
            case DocumentType.CC:
            case DocumentType.TI:
                if (!number.All(char.IsDigit) || number.Length < 6 || number.Length > 10)
                    throw new ArgumentException("CC o TI inválida.");
                break;

            case DocumentType.NIT:
                if (!Regex.IsMatch(number, @"^\d{9}-?\d$"))
                    throw new ArgumentException("NIT inválido. Ej: 900123456-1");
                break;

            case DocumentType.Passport:
                if (!Regex.IsMatch(number, @"^[A-Z0-9]{6,9}$"))
                    throw new ArgumentException("Pasaporte inválido.");
                break;

            case DocumentType.CE:
                if (!number.All(char.IsDigit) || number.Length < 6 || number.Length > 7)
                    throw new ArgumentException("CE inválida.");
                break;
        }
    }

    // ─── IEquatable<T> ────────────────────────────────────────────
    public bool Equals(Document other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type && Number == other.Number;
    }

    public override bool Equals(object obj) => Equals(obj as Document);
    public override int GetHashCode() => HashCode.Combine(Type, Number);
    public override string ToString() => $"{Type}: {Number}";

    public static bool operator ==(Document left, Document right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }
    public static bool operator !=(Document left, Document right) => !(left == right);
}