using BenchmarkDotNet.Running;
using MCIO.OutputEnvelop.Benchmarks.Interfaces;
using System.Reflection;

var benchmarkTypeFilter = new Func<Type, bool>(type =>
    typeof(IBenchmark).IsAssignableFrom(type)
    && !type.IsInterface
);

var typeSequence = 0;
var typeDictionary = Assembly
    .GetExecutingAssembly()
    .GetTypes()
    .Where(benchmarkTypeFilter)
    .OrderBy(type => type.Namespace)
    .ThenBy(type => type.Name)
    .ToDictionary(keySelector: type => typeSequence++);

var typeGroupCollection =
    from type in typeDictionary
    group type by type.Value.Namespace into typeGroup
    select new
    {
        Namespace = typeGroup.Key,
        BenchmarkTypeCollection = typeGroup.AsEnumerable()
    };

Console.WriteLine("Benchmarks:");
foreach (var typeGroup in typeGroupCollection)
{
    Console.WriteLine($"\t{typeGroup.Namespace}");

    foreach (var benchmarkType in typeGroup.BenchmarkTypeCollection)
        Console.WriteLine($"\t\t{benchmarkType.Key} - {benchmarkType.Value.Name}");
}

Console.Write("\nBenchmark code: ");
var benchmarkCode = int.Parse(Console.ReadLine()!);

BenchmarkRunner.Run(typeDictionary[benchmarkCode]!);