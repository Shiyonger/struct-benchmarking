using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using NUnit.Framework;

namespace StructBenchmarking;
public class Benchmark : IBenchmark
{
    public double MeasureDurationInMs(ITask task, int repetitionCount)
    {
        task.Run();
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Stopwatch stopwatch = new Stopwatch();
        double s = 0;
        for (int i = 0; i < repetitionCount; i++)
        {
            stopwatch.Start();
            task.Run();
            stopwatch.Stop();
            s += stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
        }

        double ans = s / repetitionCount;
        return ans;
    }
}

[TestFixture]
public class RealBenchmarkUsageSample
{
    [Test]
    public void StringConstructorFasterThanStringBuilder()
    {
        Benchmark benchmark = new Benchmark();
        StringConstructerTest sct = new StringConstructerTest();
        StringBuilderTest sbt = new StringBuilderTest();
        double sctTime = benchmark.MeasureDurationInMs(sct, 100000);
        double sbtTime = benchmark.MeasureDurationInMs(sbt, 100000);
        Assert.Less(sctTime, sbtTime);
    }
}

public class StringConstructerTest : ITask
{
    public void Run()
    {
        string s = new string('a', 10000);
    }
}

public class StringBuilderTest : ITask
{
    public void Run()
    {
        StringBuilder s = new StringBuilder();
        for (int i = 0; i < 10000; i++)
            s.Append('a');
        s.ToString();
    }
}