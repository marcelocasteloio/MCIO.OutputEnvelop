using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using MCIO.OutputEnvelop.Benchmarks.Interfaces;

namespace MCIO.OutputEnvelop.Benchmarks;

[SimpleJob(RunStrategy.Throughput)]
[GcServer(true)]
[HardwareCounters(
    HardwareCounter.CacheMisses,
    HardwareCounter.Timer,
    HardwareCounter.TotalCycles,
    HardwareCounter.TotalIssues,
    HardwareCounter.BranchMispredictions,
    HardwareCounter.BranchInstructions
)]
[MemoryDiagnoser]
public class ThrowExceptionBenchmark
    : IBenchmark
{
    private static Customer _customer = new();

    [Benchmark(Baseline = true)]
    public bool NoException()
    {
        _customer.ChangeName(name: null!, out bool success);

        return success;
    }
    [Benchmark()]
    public bool WithException()
    {
        try
        {
            _customer.ChangeNameWithThrowException(name: null!);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public class Customer
    {
        public string? Name { get; private set; }

        public void ChangeName(string name, out bool success)
        {
            if(string.IsNullOrEmpty(name) || (name.Length == 0 || name.Length > 50))
            {
                success = false;
                return;
            }

            Name = name;

            success = true;
        }

        public void ChangeNameWithThrowException(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);
            else if(name.Length == 0 || name.Length > 50)
                throw new ArgumentOutOfRangeException(name);

            Name = name;
        }
        
    }
}
