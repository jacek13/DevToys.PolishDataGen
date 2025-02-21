using DevToys.PolishDataGen.Providers.Generators;
using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class IdentityCardNumberGeneratorTests
{
    private readonly IdentityCardNumberGenerator _generator;
    private readonly IdentityCardNumberValidator _validator;

    public IdentityCardNumberGeneratorTests()
    {
        _generator = new IdentityCardNumberGenerator();
        _validator = new IdentityCardNumberValidator();
    }

    [Fact]
    public void Create_ShouldGenerateValidNip()
    {
        // Act
        var identityCardNumber = _generator.Create();

        // Assert
        Assert.True(_validator.IsValid(identityCardNumber), $"Generated Identity Card Number '{identityCardNumber}' should be valid.");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void CreateMany_ShouldGenerateValidIdentityCardNumbers(int count)
    {
        // Act
        var identityCardNumbers = _generator.CreateMany(count).ToList();

        // Assert
        Assert.Equal(count, identityCardNumbers.Count);
        Assert.All(identityCardNumbers, idc => Assert.True(_validator.IsValid(idc), $"Generated Identity Card Number '{idc}' should be valid."));
    }

    [Fact]
    public void CreateMany_ShouldGenerateUniqueIdentityCardNumberss()
    {
        // Act
        var identityCardNumbers = _generator.CreateMany(1000).ToList();

        // Assert
        var distinctCount = identityCardNumbers.Distinct().Count();
        Assert.Equal(identityCardNumbers.Count, distinctCount);
    }

    [Fact]
    public void Create_ShouldGenerateIdentityCardNumbersWithValidControlNumber()
    {
        // Act
        var identityCardNumber = _generator.Create();
        var controlNumber = _validator.CalculateControlNumber(identityCardNumber);

        // Assert
        Assert.Equal(0, controlNumber);
    }
}
