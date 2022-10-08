using System.Collections.Generic;
using System;

namespace StructBenchmarking
{
    public abstract class CharDataBuilder
    {
        string title;

        private void InitObjects(ref ITask createdStruct, 
            ref ITask createdClass, int fieldsCount)
        {
            if (this is ArrayCreation)
            {
                createdStruct = new StructArrayCreationTask(fieldsCount);
                createdClass = new ClassArrayCreationTask(fieldsCount);
            }
            else
            {
                createdStruct = new MethodCallWithStructArgumentTask(fieldsCount);
                createdClass = new MethodCallWithClassArgumentTask(fieldsCount);
            }
        }

        public ChartData BuildChartData(IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();
            foreach (var fieldsCount in Constants.FieldCounts)
            {
                ITask createdStruct = null, createdClass = null;
                InitObjects(ref createdStruct, ref createdClass, fieldsCount);
                var structAverageTime = benchmark
                    .MeasureDurationInMs(createdStruct, repetitionsCount);
                var classAverageTime = benchmark
                    .MeasureDurationInMs(createdClass, repetitionsCount);
                classesTimes.Add(new ExperimentResult(fieldsCount, classAverageTime));
                structuresTimes.Add(new ExperimentResult(fieldsCount, structAverageTime));
            }
            if (this is ArrayCreation) title = "Create array";
            else title = "Call method with argument";
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