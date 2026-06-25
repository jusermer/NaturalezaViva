namespace NaturalezaViva.Domain.Exceptions;

/// <summary>
/// Se lanza cuando un recurso solicitado no existe en el sistema.
/// Hereda de DomainException para mantenerse dentro de la jerarquía de excepciones de dominio,
/// pero se distingue como un caso semánticamente distinto (404) frente a otras violaciones
/// de reglas de negocio (400/409).
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception inner) : base(message, inner) { }
}