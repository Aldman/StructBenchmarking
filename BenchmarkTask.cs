using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            task.Run();
            var commonMeasuring = new Stopwatch();
            for (int i = 0; i < repetitionCount; i++)
            {
                commonMeasuring.Start();
                task.Run();
                commonMeasuring.Stop();
            }
            return (double)commonMeasuring
                .ElapsedMilliseconds / repetitionCount;
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var strBld = new StringBuilderCreator();
            var str = new StringCreator();
            var bench = new Benchmark();
            var repeatCount = 10000;
            var strResult = bench.MeasureDurationInMs(str, repeatCount);
            var strBldResult = bench.MeasureDurationInMs(strBld, repeatCount);
            Assert.Less(strResult, strBldResult);
        }
    }

    public class StringCreator : ITask
    {
        public void Run()
        {
            var str = new string('a', 10000);
        }
    }

    public class StringBuilderCreator : ITask
    {
        public void Run()
        {
            var strBldg = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                strBldg.Append('a');
            }
            strBldg.ToString();
        }
    }
}