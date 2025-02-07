using DevToys.PolishDataGen.Providers.Generators;
using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class RegonLongGeneratorTests
{
    private readonly RegonGeneratorBase _generator;
    private readonly RegonValidator _validator;

    public RegonLongGeneratorTests()
    {
        _generator = new RegonLongGenerator();
        _validator = new RegonValidator();
    }

    [Fact]
    public void Create_ShouldGenerateValidRegon()
    {
        // Act
        var regon = _generator.Create();

        // Assert
        Assert.True(_validator.IsValid(regon), $"Generated 14-digit REGON '{regon}' should be valid.");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void CreateMany_ShouldGenerateValidRegons(int count)
    {
        // Act
        var regons = _generator.CreateMany(count).ToList();

        // Assert
        Assert.Equal(count, regons.Count);
        foreach (var regon in regons)
        {
            Assert.True(_validator.IsValid(regon), $"Generated 14-digit REGON '{regon}' should be valid.");
        }
    }

    [Fact]
    public void Create_ShouldGenerateUniqueRegons()
    {
        // Act
        var regons = _generator.CreateMany(1000).ToList();

        // Assert
        var distinctCount = regons.Distinct().Count();
        Assert.Equal(regons.Count, distinctCount);
    }

    [Fact]
    public void Create_ShouldGenerateRegonWithValidControlNumber()
    {
        // Act
        var regon = _generator.Create();
        var controlNumber = _validator.CalculateControlNumber(regon);
        var actualControlNumber = regon[^1] - '0'; // Last digit

        // Assert
        Assert.Equal(controlNumber, actualControlNumber);
    }
}
