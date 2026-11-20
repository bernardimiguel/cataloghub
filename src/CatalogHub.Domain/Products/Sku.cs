using System.Text.RegularExpressions;

namespace CatalogHub.Domain.Products;

public sealed partial record Sku
{
    private const int MaxLength = 50;

    public string Value { get; }

    private Sku(string value) => Value = value;

    public static Sku Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("SKU cannot be null or empty.", nameof(value));
        }

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > MaxLength)
        {
            throw new ArgumentException($"SKU cannot exceed {MaxLength} characters.", nameof(value));
        }

        if (!AllowedCharacters().IsMatch(normalized))
        {
            throw new ArgumentException("SKU contains invalid characters. Only letters, numbers, and hyphens are allowed.", nameof(value));
        }

        return new Sku(normalized);
    }

    public static Sku Generate() => Create(Guid.NewGuid().ToString());

    [GeneratedRegex("^[A-Z0-9-]+$")]
    private static partial Regex AllowedCharacters();
}
