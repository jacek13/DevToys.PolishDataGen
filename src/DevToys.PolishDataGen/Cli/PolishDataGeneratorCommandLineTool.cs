using DevToys.Api;
using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Generators;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Text;

namespace DevToys.PolishDataGen.Cli;

[Export(typeof(ICommandLineTool))]
[Name(nameof(PolishDataGeneratorCommandLineTool))]
[CommandName(
    Name = "polish-data-gen",
    Alias = "pdg",
    ResourceManagerBaseName = "DevToys.PolishDataGen.Strings.PolishDataGen",
    DescriptionResourceName = nameof(Strings.PolishDataGen.Description))]
internal class PolishDataGeneratorCommandLineTool : ICommandLineTool
{
    [CommandLineOption(Name = "type", Alias = "t", DescriptionResourceName = nameof(Strings.PolishDataGen.CliGeneratorType))]
    internal string Type { get; set; } = string.Empty;

    [CommandLineOption(Name = "output", Alias = "o", DescriptionResourceName = nameof(Strings.PolishDataGen.CliOutputFilePath))]
    internal string Output { get; set; } = string.Empty;

    [CommandLineOption(Name = "multithreading", Alias = "mt", DescriptionResourceName = nameof(Strings.PolishDataGen.CliEnableMultithreading))]
    internal bool EnableMultithreading { get; set; }

    [CommandLineOption(Name = "number", Alias = "n", DescriptionResourceName = nameof(Strings.PolishDataGen.CliNumberPropertyDescription))]
    internal int Number { get; set; }

    private GeneratorType _generatorType { get; set; } = GeneratorType.Unknown;

    public async ValueTask<int> InvokeAsync(ILogger logger, CancellationToken cancellationToken)
    {
        _generatorType = GeneratorTypeHelper.ConvertToGeneratorType(Type);

        var errorMessages = ValidateInputs().ToList();
        if (errorMessages.Any())
        {
            errorMessages.ForEach(Console.Error.WriteLine);
            return -1;
        }

        IPolishIdGenerator generator = GeneratorFactory.Create(_generatorType);
        var results = new ConcurrentBag<string>();
        var timer = Stopwatch.StartNew();

        if (Number == 1)
        {
            var result = generator.Create();
            results.Add(result);
            timer.Stop();
        }
        else if (Number > 1 && EnableMultithreading)
        {
            var numberOfTasks = CalculateNumberOfIntervals(Number);
            var intervalLength = (int)Math.Ceiling((double)Number / (double)numberOfTasks);
            var tasks = new List<Task>(numberOfTasks);

            for (int i = 0; i < numberOfTasks; i++)
            {
                tasks.Add(GeneratePart(results, intervalLength));
            }

            await Task.WhenAll(tasks);
            timer.Stop();
        }
        else
        {
            var ids = generator.CreateMany(Number);
            results = new ConcurrentBag<string>(ids);
            timer.Stop();
        }
        Console.WriteLine($"Finished in {timer.Elapsed} ms");

        if (!string.IsNullOrWhiteSpace(Output))
        {
            var destination = new StringBuilder()
                .Append(Output)
                .Append(Output[^1] is '\\' or '/' ? string.Empty : '\\')
                .AppendFormat("generated-{0}-{1}-", Number, _generatorType.ToString().ToLower())
                .AppendFormat("in-{0}-ms", timer.ElapsedMilliseconds)
                .Append(".txt")
                .ToString();

            Console.WriteLine($"Saving results in '{destination}'");
            await SaveAsync(destination, results, cancellationToken);
        }
        else
        {
            PrintResults(results);
        }

        return 0;
    }

    private async Task GeneratePart(ConcurrentBag<string> results, int intervalLength)
    {
        var ids = await GenerateMany(_generatorType, intervalLength);
        foreach (var id in ids)
        {
            results.Add(id);
        }
    }

    private IEnumerable<string> ValidateInputs()
    {
        if (_generatorType == GeneratorType.Unknown) yield return $"Property '{nameof(Type)}' is null or unknown type";
        if (Number < 1) yield return $"Property '{nameof(Number)}' is less than 1";
    }

    private static Task<IEnumerable<string>> GenerateMany(GeneratorType Type, int count)
        => Type switch
        {
            GeneratorType.Pesel => Task.FromResult(new PeselGenerator().CreateMany(count)),
            GeneratorType.Regon => Task.FromResult(new RegonGenerator().CreateMany(count)),
            GeneratorType.RegonLong => Task.FromResult(new RegonLongGenerator().CreateMany(count)),
            GeneratorType.Nip => Task.FromResult(new NipGenerator().CreateMany(count)),
            _ => Task.FromResult(Enumerable.Empty<string>())
        };

    private static int CalculateNumberOfIntervals(int count)
        => Convert.ToInt32(Math.Round(1.0 + 3.3 * Math.Log10(count)));

    private static async Task SaveAsync(string path, IEnumerable<string> results, CancellationToken cancellationToken)
        => await File.WriteAllLinesAsync(path, results, Encoding.UTF8, cancellationToken);

    private static void PrintResults(IEnumerable<string> results)
    {
        foreach (string id in results)
        {
            Console.WriteLine(id);
        }
    }
}
