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
public class CreateOutputEnvelopBenchmark
    : IBenchmark
{
    // Fields
    private static readonly string _outputMessageCode = new('a', 50);
    private static readonly OutputEnvelop _outputEnvelop = OutputEnvelop.CreateSuccess(
        _outputMessageCollection,
        exceptionCollection: null
    );
    private static readonly OutputEnvelop<object> _outputEnvelopWithOutput = OutputEnvelop<object>.CreateSuccess(
        output: null!,
        _outputMessageCollection,
        exceptionCollection: null
    );
    private static readonly OutputEnvelop[] _outputEnvelopCollection = [
        _outputEnvelop,
        _outputEnvelop
    ];
    private static readonly OutputEnvelop<object>[] _outputEnvelopWithOutputCollection = [
        _outputEnvelop,
        _outputEnvelop
    ];
    private static readonly OutputMessage[] _outputMessageCollection = [
        OutputMessage.CreateInformation(_outputMessageCode),
        OutputMessage.CreateInformation(_outputMessageCode),
        OutputMessage.CreateInformation(_outputMessageCode),
        OutputMessage.CreateInformation(_outputMessageCode),
        OutputMessage.CreateInformation(_outputMessageCode)
    ];

    // Properties
    [Params(10)]
    public int OutputEnvelopCount { get; set; }

    // Public Methods
    [Benchmark(Baseline = true)]
    public OutputEnvelop CreateOutputEnvelopFromTypeWithoutMessageAndException()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop.Create(
                type: Enums.OutputEnvelopType.Success
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop<object> CreateOutputEnvelopWithOutputFromTypeWithoutMessageAndException()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop<object>.Create(
                output: null!,
                type: Enums.OutputEnvelopType.Success
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop CreateOutputEnvelopWithoutMessageAndException()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop.CreateSuccess();
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop<object> CreateOutputEnvelopWithOutputWithoutMessageAndException()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop<object>.CreateSuccess(output: null!);
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop CreateOutputEnvelopWithExistingMessageCollection()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop.CreateSuccess(
                outputMessageCollection: _outputMessageCollection,
                exceptionCollection: null
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop<object> CreateOutputEnvelopWithOutputWithExistingMessageCollection()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop<object>.CreateSuccess(
                output: null!,
                outputMessageCollection: _outputMessageCollection,
                exceptionCollection: null
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop CreateOutputEnvelopMessageCollection()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop.CreateSuccess(
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>([
                    OutputMessage.CreateInformation(_outputMessageCode),
                    OutputMessage.CreateInformation(_outputMessageCode),
                    OutputMessage.CreateInformation(_outputMessageCode)
                ]),
                exceptionCollection: null
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop<object> CreateOutputEnvelopWithOutputMessageCollection()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop<object>.CreateSuccess(
                output: null!,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>([
                    OutputMessage.CreateInformation(_outputMessageCode),
                    OutputMessage.CreateInformation(_outputMessageCode),
                    OutputMessage.CreateInformation(_outputMessageCode)
                ]),
                exceptionCollection: null
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop CreateOutputEnvelopFromAnotherOutputEnvelop()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop.CreateSuccess(_outputEnvelop);
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop<object> CreateOutputEnvelopWithOutputFromAnotherOutputEnvelop()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop<object>.CreateSuccess(
                output: null!,
                _outputEnvelop
            );
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop CreateOutputEnvelopFromAnotherOutputEnvelopCollection()
    {
        var lastOutputEnvelop = default(OutputEnvelop);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop.CreateSuccess(_outputEnvelopCollection);
        }

        return lastOutputEnvelop;
    }

    [Benchmark]
    public OutputEnvelop<object> CreateOutputEnvelopWithOutputFromAnotherOutputEnvelopCollection()
    {
        var lastOutputEnvelop = default(OutputEnvelop<object>);

        for (int i = 0; i < OutputEnvelopCount; i++)
        {
            lastOutputEnvelop = OutputEnvelop<object>.CreateSuccess(
                output: null!,
                _outputEnvelopWithOutputCollection
            );
        }

        return lastOutputEnvelop;
    }
}
