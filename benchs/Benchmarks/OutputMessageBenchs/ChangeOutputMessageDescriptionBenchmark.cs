using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using MCIO.OutputEnvelop.Benchmarks.Interfaces;
using MCIO.OutputEnvelop.Models;

namespace MCIO.OutputEnvelop.Benchmarks.OutputMessageBenchs;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters(
    HardwareCounter.CacheMisses,
    HardwareCounter.Timer,
    HardwareCounter.TotalCycles,
    HardwareCounter.TotalIssues,
    HardwareCounter.BranchMispredictions,
    HardwareCounter.BranchInstructions
)]
[MemoryDiagnoser]
public class ChangeOutputMessageDescriptionBenchmark
    : IBenchmark
{
    // Fields
    private static readonly OutputMessage _outputMessage = OutputMessage.CreateSuccess(code: new string('a', 50));
    private static readonly string _newDescription = new('a', 255);

    // Properties
    [Params(1, 5, 10)]
    public int OutputMessageCount { get; set; }

    // Public Methods
    [Benchmark()]
    public OutputMessage ChangeOutputMessageDescription()
    {
        var lastOutputMessage = default(OutputMessage);

        for (int i = 0; i < OutputMessageCount; i++)
        {
            lastOutputMessage = _outputMessage.ChangeDescription(_newDescription);
        }

        return lastOutputMessage;
    }
}

