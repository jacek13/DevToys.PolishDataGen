using DevToys.PolishDataGen.Providers.Generators;
using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class PeselGeneratorTests
{
    private readonly PeselGenerator _generator;
    private readonly PeselValidator _validator;

    public PeselGeneratorTests()
    {
        _generator = new PeselGenerator();
        _validator = new PeselValidator();
    }

    [Fact]
    public void Create_ShouldGenerateValidPesel()
    {
        // Act
        var pesel = _generator.Create();

        // Assert
        Assert.True(_validator.IsValid(pesel), $"Generated PESEL '{pesel}' should be valid.");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void CreateMany_ShouldGenerateValidPesels(int count)
    {
        // Act
        var pesels = _generator.CreateMany(count).ToList();

        // Assert
        Assert.Equal(count, pesels.Count);
        Assert.All(pesels, pesel => Assert.True(_validator.IsValid(pesel), $"Generated PESEL '{pesel}' should be valid."));
    }

    [Fact]
    public void CreateMany_ShouldGenerateUniquePesels()
    {
        // Act
        var pesels = _generator.CreateMany(1000).ToList();

        // Assert
        var distinctCount = pesels.Distinct().Count();
        Assert.Equal(pesels.Count, distinctCount);
    }

    [Fact]
    public void Create_ShouldGeneratePeselWithValidControlNumber()
    {
        // Act
        var pesel = _generator.Create();
        var controlNumber = _validator.CalculateControlNumber(pesel);
        var actualControlNumber = pesel[^1] - '0'; // Last digit

        // Assert
        Assert.Equal(controlNumber, actualControlNumber);
    }
}
