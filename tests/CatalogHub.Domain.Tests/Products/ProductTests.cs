using CatalogHub.Domain.Products;

namespace CatalogHub.Domain.Tests.Products;

public sealed class ProductTests
{
    private const int NameMaxLength = 100;
    private const decimal MinimumPrice = 0.01m;
    private const string ValidName = "Sample Product";
    private const string ValidDescription = "This is a sample product.";
    private const decimal ValidPrice = 19.99m;
    private const int ValidStock = 100;

    private readonly Sku _validSku = Sku.Create("HW-1968");

    [Fact]
    public void Create_WithValidData_GeneratesId()
    {
        var product = Product.Create(_validSku, ValidName, ValidDescription, ValidPrice, ValidStock);

        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public void Create_WithValidData_CreatesProduct()
    {
        var product = Product.Create(_validSku, ValidName, ValidDescription, ValidPrice, ValidStock);

        Assert.Equal(_validSku, product.Sku);
        Assert.Equal(ValidName, product.Name);
        Assert.Equal(ValidDescription, product.Description);
        Assert.Equal(ValidPrice, product.Price);
        Assert.Equal(ValidStock, product.Stock);
    }

    [Fact]
    public void Create_WithNullSku_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Product.Create(null!, ValidName, ValidDescription, ValidPrice, ValidStock));
    }

    [Fact]
    public void Create_WithNameMatchingMaxLength_CreatesProduct()
    {
        var longName = new string('A', NameMaxLength);

        var product = Product.Create(_validSku, longName, ValidDescription, ValidPrice, ValidStock);

        Assert.Equal(longName, product.Name);
    }

    [Fact]
    public void Create_WithLeadingAndTrailingWhitespace_TrimsName()
    {
        var product = Product.Create(_validSku, " Sample Product ", ValidDescription, ValidPrice, ValidStock);

        Assert.Equal(ValidName, product.Name);
    }

    [Fact]
    public void Create_WithNullName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Product.Create(_validSku, null!, ValidDescription, ValidPrice, ValidStock));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ThrowsArgumentException(string invalidName)
    {
        Assert.Throws<ArgumentException>(() => Product.Create(_validSku, invalidName, ValidDescription, ValidPrice, ValidStock));
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ThrowsArgumentException()
    {
        var longName = new string('A', NameMaxLength + 1);

        Assert.Throws<ArgumentException>(() => Product.Create(_validSku, longName, ValidDescription, ValidPrice, ValidStock));
    }

    [Fact]
    public void Create_WithLeadingAndTrailingWhitespace_TrimsDescription()
    {
        var product = Product.Create(_validSku, ValidName, "  This is a sample product.  ", ValidPrice, ValidStock);

        Assert.Equal(ValidDescription, product.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyDescription_SetsDescriptionToNull(string? description)
    {
        var product = Product.Create(_validSku, ValidName, description, ValidPrice, ValidStock);

        Assert.Null(product.Description);
    }

    [Fact]
    public void Create_WithMinimumPrice_CreatesProduct()
    {
        var product = Product.Create(_validSku, ValidName, ValidDescription, MinimumPrice, ValidStock);

        Assert.Equal(MinimumPrice, product.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidPrice_ThrowsArgumentOutOfRangeException(decimal invalidPrice)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Product.Create(_validSku, ValidName, ValidDescription, invalidPrice, ValidStock));
    }

    [Fact]
    public void Create_WithZeroStock_CreatesProduct()
    {
        var product = Product.Create(_validSku, ValidName, ValidDescription, ValidPrice, 0);

        Assert.Equal(0, product.Stock);
    }

    [Fact]
    public void Create_WithInvalidStock_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Product.Create(_validSku, ValidName, ValidDescription, ValidPrice, -1));
    }
}
