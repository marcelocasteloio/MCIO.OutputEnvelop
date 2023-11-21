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
public class CreateOutputMessageBenchmark
    : IBenchmark
{
    // Fields
    private static readonly string _outputMessageCode = new('a', 50);
    private static readonly string _outputMessageDescription = new('a', 250);

    // Properties
    [Params(1, 5, 10, 20)]
    public int OutputMessageCount { get; set; }

    // Public Methods
    [Benchmark(Baseline = true)]
    public OutputMessage CreateOutputMessageFromTypeWithoutDescription()
    {
        var lastOutputMessage = default(OutputMessage);

        for (int i = 0; i < OutputMessageCount; i++)
        {
            lastOutputMessage = OutputMessage.Create(
                type: Enums.OutputMessageType.Success,
                code: _outputMessageCode,
                description: null
            );
        }

        return lastOutputMessage;
    }

    [Benchmark()]
    public OutputMessage CreateOutputMessageFromType()
    {
        var lastOutputMessage = default(OutputMessage);

        for (int i = 0; i < OutputMessageCount; i++)
        {
            lastOutputMessage = OutputMessage.Create(
                type: Enums.OutputMessageType.Success,
                code: _outputMessageCode,
                description: _outputMessageDescription
            );
        }

        return lastOutputMessage;
    }

    [Benchmark()]
    public OutputMessage CreateOutputMessageWithoutDescription()
    {
        var lastOutputMessage = default(OutputMessage);

        for (int i = 0; i < OutputMessageCount; i++)
        {
            lastOutputMessage = OutputMessage.CreateSuccess(
                code: _outputMessageCode,
                description: null
            );
        }

        return lastOutputMessage;
    }

    [Benchmark()]
    public OutputMessage CreateOutputMessage()
    {
        var lastOutputMessage = default(OutputMessage);

        for (int i = 0; i < OutputMessageCount; i++)
        {
            lastOutputMessage = OutputMessage.CreateSuccess(
                code: _outputMessageCode,
                description: _outputMessageDescription
            );
        }

        return lastOutputMessage;
    }
}
