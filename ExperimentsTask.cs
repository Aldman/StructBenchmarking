using System.Collections.Generic;
using System;

namespace StructBenchmarking
{
    public abstract class CharDataBuilder
    {
        string title;

        public ChartData BuildChartData(IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var fieldsCount in Constants.FieldCounts)
            {
                var createdStruct = new StructArrayCreationTask(fieldsCount);
                var createdClass = new ClassArrayCreationTask(fieldsCount);
                double structAverageTime = 0, classAverageTime = 0;

                if (this is ArrayCreation)
                {
                    structAverageTime = benchmark
                    .MeasureDurationInMs(createdStruct, repetitionsCount);
                    classAverageTime = benchmark
                        .MeasureDurationInMs(createdClass, repetitionsCount);
                    title = "Create array";
                }
                else if (this is MethodCall)
                {
                    structAverageTime = benchmark
                    .MeasureDurationInMs(createdStruct, repetitionsCount);
                    classAverageTime = benchmark
                        .MeasureDurationInMs(createdClass, repetitionsCount);
                    title = "Call method with argument";
                }

                classesTimes.Add(new ExperimentResult(fieldsCount, classAverageTime));
                structuresTimes.Add(new ExperimentResult(fieldsCount, structAverageTime));
            }

            return new ChartData
            {
                Title = title,
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }

    public class ArrayCreation : CharDataBuilder { }
    public class MethodCall : CharDataBuilder { }
    
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            CharDataBuilder charDataBuilder = new ArrayCreation();
            return charDataBuilder.BuildChartData(benchmark, repetitionsCount);
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            CharDataBuilder charDataBuilder = new MethodCall();
            return charDataBuilder.BuildChartData(benchmark, repetitionsCount);
        }
    }
}