﻿using DevToys.PolishDataGen.Providers.Generators;
using DevToys.PolishDataGen.Providers.Validators;

namespace DevToys.PolishDataGen.Test;

public class RegonGeneratorTests
{
    private readonly RegonGeneratorBase _generator;
    private readonly RegonValidator _validator;

    public RegonGeneratorTests()
    {
        _generator = new RegonGenerator();
        _validator = new RegonValidator();
    }

    [Fact]
    public void Create_ShouldGenerateValidRegon()
    {
        // Act
        var regon = _generator.Create();

        // Assert
        Assert.True(_validator.IsValid(regon), $"Generated REGON '{regon}' should be valid.");
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void CreateMany_ShouldGenerateValidRegons(int count)
    {
        // Act
        var regons = _generator.CreateMany(count).ToList();

        // Assert
        Assert.Equal(count, regons.Count);
        Assert.All(regons, regon => Assert.True(_validator.IsValid(regon), $"Generated REGON '{regon}' should be valid."));
    }

    [Fact]
    public void CreateMany_ShouldGenerateUniqueRegons()
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
