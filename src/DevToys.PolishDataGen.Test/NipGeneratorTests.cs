using DevToys.PolishDataGen.Providers.Generators;
using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class NipGeneratorTests
{
    private readonly NipGenerator _generator;
    private readonly NipValidator _validator;

    public NipGeneratorTests()
    {
        _generator = new NipGenerator();
        _validator = new NipValidator();
    }

    [Fact]
    public void Create_ShouldGenerateValidNip()
    {
        // Act
        var nip = _generator.Create();

        // Assert
        Assert.True(_validator.IsValid(nip), $"Generated NIP '{nip}' should be valid.");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void CreateMany_ShouldGenerateValidNips(int count)
    {
        // Act
        var nips = _generator.CreateMany(count).ToList();

        // Assert
        Assert.Equal(count, nips.Count);
        Assert.All(nips, nip => Assert.True(_validator.IsValid(nip), $"Generated NIP '{nip}' should be valid."));
    }

    [Fact]
    public void Create_ShouldGenerateUniqueNips()
    {
        // Act
        var nips = _generator.CreateMany(1000).ToList();

        // Assert
        var distinctCount = nips.Distinct().Count();
        Assert.Equal(nips.Count, distinctCount);
    }

    [Fact]
    public void Create_ShouldGenerateNipWithValidControlNumber()
    {
        // Act
        var nip = _generator.Create();
        var controlNumber = _validator.CalculateControlNumber(nip);
        var actualControlNumber = nip[^1] - '0'; // Last digit

        // Assert
        Assert.Equal(controlNumber, actualControlNumber);
    }
}
