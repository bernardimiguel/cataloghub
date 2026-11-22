using CatalogHub.Domain.Products;

namespace CatalogHub.Domain.Tests.Products;

public sealed class SkuTests
{
    private const int SkuMaxLength = 50;

    [Theory]
    [InlineData("HW-1968")]
    [InlineData("hw-1968")]
    [InlineData("Hw-1968")]
    [InlineData(" HW-1968 ")]
    public void Create_WithValidValue_ReturnsNormalizedValue(string input)
    {
        var sku = Sku.Create(input);

        Assert.Equal("HW-1968", sku.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("HW 1968")]
    [InlineData("HW_1968")]
    [InlineData("AÇÃO")]
    public void Create_WithInvalidValue_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => Sku.Create(input));
    }

    [Fact]
    public void Create_WithMaxLengthValue_ReturnsSameValue()
    {
        var input = new string('A', SkuMaxLength);
        var sku = Sku.Create(input);

        Assert.Equal(input, sku.Value);
    }

    [Fact]
    public void Create_WithExceedingMaxLengthValue_ThrowsArgumentException()
    {
        var input = new string('A', SkuMaxLength + 1);

        Assert.Throws<ArgumentException>(() => Sku.Create(input));
    }

    [Fact]
    public void Generate_WhenCalled_ReturnsValidSku()
    {
        var sku = Sku.Generate();

        Assert.Matches(@"^[A-Z0-9-]+$", sku.Value);
    }

    [Fact]
    public void Generate_WhenCalledTwice_ReturnsDifferentValues()
    {
        var sku1 = Sku.Generate();
        var sku2 = Sku.Generate();

        Assert.NotEqual(sku1, sku2);
    }
}
