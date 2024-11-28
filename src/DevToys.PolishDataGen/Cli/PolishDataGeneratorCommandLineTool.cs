using DevToys.Api;
using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Generators;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Diagnostics;

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
    internal GeneratorType Type { get; set; }

    [CommandLineOption(Name = "multithreading", Alias = "mt", DescriptionResourceName = nameof(Strings.PolishDataGen.CliEnableMultithreading))]
    internal bool EnableMultithreading { get; set; }

    [CommandLineOption(Name = "number", Alias = "n", DescriptionResourceName = nameof(Strings.PolishDataGen.CliNumberPropertyDescription))]
    internal int Number { get; set; }

    [CommandLineOption(Name = "output", Alias = "o", DescriptionResourceName = nameof(Strings.PolishDataGen.CliOutputFilePath))]
    internal string Output { get; set; }

    public async ValueTask<int> InvokeAsync(ILogger logger, CancellationToken cancellationToken)
    {
        // TODO Refactor it later. Split it to private static functions
        if (Type == GeneratorType.Unknown)
        {
            Console.Error.WriteLine($"Property '{nameof(Type)}' is null or unknown type");
            return -1;
        }

        if (Number < 1)
        {
            Console.Error.WriteLine($"Property '{nameof(Number)}' is less than 1");
            return -1;
        }

        // TODO factory that return generator
        IPolishIdGenerator generator = new PeselGenerator();
        var results = new ConcurrentBag<string>();
        var timer = Stopwatch.StartNew();

        if (Number > 1)
        {
            if (EnableMultithreading)
            {
                var numberOfTasks = CalculateNumberOfIntervals(Number);
                var intervalLength = (double)Number / (double)numberOfTasks;
                var tasks = new List<Task>(numberOfTasks);

                for (int i = 0; i < numberOfTasks; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var ids = await GenerateMany(Type, (int)Math.Ceiling(intervalLength));
                        foreach (var id in ids)
                        {
                            results.Add(id);
                        }
                    }));
                }

                await Task.WhenAll(tasks);
                timer.Stop();
                Console.WriteLine($"Finished in {timer.Elapsed} ms");
                // TODO make print function if output is not defined
                // TODO check memory allocation
                //PrintResults(results);
            }
            else
            {
                var ids = generator.CreateMany(Number);
                timer.Stop();
                Console.WriteLine($"Finished in {timer.Elapsed} ms");
                //PrintResults(ids);
            }
        }
        else
        {
            var result = generator.Create();
            timer.Stop();
            Console.WriteLine($"Finished in {timer.Elapsed} ms");
            //Console.WriteLine(result);
        }

        return 0; // Exit code.
    }

    private static Task<IEnumerable<string>> GenerateMany(GeneratorType Type, int count)
        => Type switch
        {
            GeneratorType.Pesel => Task.FromResult(new PeselGenerator().CreateMany(count)),
            _ => Task.FromResult(Enumerable.Empty<string>())
        };

    private static int CalculateNumberOfIntervals(int count)
        => Convert.ToInt32(Math.Round(1.0 + 3.3 * Math.Log10(count)));

    private void PrintResults(IEnumerable<string> results)
    {
        foreach (string id in results)
        {
            Console.WriteLine(id);
        }
    }
}
