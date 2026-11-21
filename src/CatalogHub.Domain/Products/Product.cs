namespace CatalogHub.Domain.Products;

public sealed class Product
{
    private const int MaxNameLength = 100;

    public Guid Id { get; }

    public Sku Sku { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    private Product(Guid id, Sku sku, string name, string? description, decimal price, int stock)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }

    public static Product Create(Sku sku, string name, string? description, decimal price, int stock)
    {
        ArgumentNullException.ThrowIfNull(sku);
        ArgumentNullException.ThrowIfNull(name);

        var normalizedName = name.Trim();

        if (normalizedName.Length == 0)
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        }

        if (normalizedName.Length > MaxNameLength)
        {
            throw new ArgumentException($"Product name cannot exceed {MaxNameLength} characters.", nameof(name));
        }

        var normalizedDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Product price cannot be zero or negative.");
        }

        if (stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stock), "Product stock cannot be negative.");
        }

        return new Product(Guid.NewGuid(), sku, normalizedName, normalizedDescription, price, stock);
    }
}
