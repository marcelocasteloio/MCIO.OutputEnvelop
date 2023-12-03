using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using MCIO.OutputEnvelop.Benchmarks.Interfaces;
using MCIO.OutputEnvelop.Models;

namespace MCIO.OutputEnvelop.Benchmarks.OutputEnvelopBenchs;

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
public class ChangeOutputMessageTypeBenchmark
    : IBenchmark
{
    // Fields
    private static readonly OutputEnvelop _outputEnvelop = OutputEnvelop.CreateSuccess(
        outputMessageCollection: [
            OutputMessage.CreateInformation("A"),
            OutputMessage.CreateInformation("B"),
            OutputMessage.CreateInformation("C"),
            OutputMessage.CreateInformation("D"),
            OutputMessage.CreateInformation("E")
        ],
        exceptionCollection: null
    );
    private static readonly OutputEnvelop _outputEnvelopWithOutput = OutputEnvelop<object>.CreateSuccess(
        output: null!,
        outputMessageCollection: [
            OutputMessage.CreateInformation("A"),
            OutputMessage.CreateInformation("B"),
            OutputMessage.CreateInformation("C"),
            OutputMessage.CreateInformation("D"),
            OutputMessage.CreateInformation("E")
        ],
        exceptionCollection: null
    );

    // Properties
    [Params(10)]
    public int OutputEnvelopCount { get; set; }

    // Public Methods
    [Benchmark(Baseline = true)]
    public OutputEnvelop OutputEnvelopChangeOutputMessageType()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = _outputEnvelop.ChangeOutputMessageType("C", Enums.OutputMessageType.Error);
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop OutputEnvelopWithOutputChangeOutputMessageType()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = _outputEnvelopWithOutput.ChangeOutputMessageType("C", Enums.OutputMessageType.Error);
        }

        return lastOutputEnvelop;
    }
}
