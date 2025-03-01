using System.ComponentModel.Composition;
using DevToys.Api;
using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Common;
using DevToys.PolishDataGen.Providers.Validators;
using Microsoft.Extensions.Logging;

namespace DevToys.PolishDataGen.Cli;

[Export(typeof(ICommandLineTool))]
[Name(nameof(PolishDataValidatorCommandLineTool))]
[CommandName(
    Name = "polish-data-validator",
    Alias = "pdv",
    ResourceManagerBaseName = "DevToys.PolishDataGen.Strings.PolishDataGen",
    DescriptionResourceName = nameof(Strings.PolishDataGen.CliValidatorDescription))]
internal class PolishDataValidatorCommandLineTool : ICommandLineTool
{
    [CommandLineOption(Name = "input", Alias = "i", DescriptionResourceName = nameof(Strings.PolishDataGen.CliValidatorInput))]
    internal string Input { get; set; } = string.Empty;

    [CommandLineOption(Name = "type", Alias = "t", DescriptionResourceName = nameof(Strings.PolishDataGen.CliIdType))]
    internal string Type { get; set; } = string.Empty;

    [CommandLineOption(Name = "detailed", Alias = "d", DescriptionResourceName = nameof(Strings.PolishDataGen.CliValidatorExtendedDetailsDescription))]
    internal bool ShowExtendedDetails { get; set; }

    [CommandLineOption(Name = "return-integer", Alias = "ri", DescriptionResourceName = nameof(Strings.PolishDataGen.CliValidatorIntegerAsResult))]
    internal bool ReturnInteger { get; set; }

    private IdType _idType { get; set; } = IdType.Unknown;

    public ValueTask<int> InvokeAsync(ILogger logger, CancellationToken cancellationToken)
    {
        _idType = IdTypeHelper.ConvertToIdType(Type);

        var errorMessages = ValidateInputs().ToList();
        if (errorMessages.Any())
        {
            errorMessages.ForEach(Console.Error.WriteLine);
            return ValueTask.FromResult(-1);
        }

        IPolishIdValidator validator = ValidatorFactory.Create(_idType);
        if (ReturnInteger)
            return ValueTask.FromResult(Convert.ToInt32(validator.IsValid(Input)));

        if (ShowExtendedDetails)
        {
            PrintResult(validator.IsValidExplained(Input));
        }
        else
        {
            PrintResult(validator.IsValid(Input));
        }

        return ValueTask.FromResult(0);
    }

    private IEnumerable<string> ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(Input)) 
            yield return $"Property '{nameof(Input)}' is null or unknown type";
        if (_idType == IdType.Unknown) 
            yield return $"Property '{nameof(Type)}' is null or unknown type";
    }

    private void PrintResult(bool isValid)
        => Console.WriteLine($"Validation result for '{Input}' using '{_idType}': {(isValid ? "Valid" : "Invalid")}");

    private void PrintResult((bool isValid, IEnumerable<string> errorMessages) pair)
    {
        PrintResult(pair.isValid);
        Console.WriteLine();

        if (!pair.isValid)
        {
            pair.errorMessages.ToList().ForEach(Console.Error.WriteLine);
        }
    }
}
