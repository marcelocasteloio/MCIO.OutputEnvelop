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
public class ChangeOutputMessageTypeBenchmark
    : IBenchmark
{
    private static readonly OutputMessage _outputMessage = OutputMessage.CreateSuccess(code: new string('a', 50));

    [Params(1, 5)]
    public int OutputMessageCount { get; set; }

    [Benchmark()]
    public OutputMessage ChangeOutputMessageType()
    {
        var lastOutputMessage = default(OutputMessage);

        for (int i = 0; i < OutputMessageCount; i++)
        {
            lastOutputMessage = _outputMessage.ChangeType(Enums.OutputMessageType.Error);
        }

        return lastOutputMessage;
    }
}
