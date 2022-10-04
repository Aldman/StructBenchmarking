using System.Collections.Generic;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var fieldsCount in Constants.FieldCounts)
            {
                var createdStruct = new StructArrayCreationTask(fieldsCount);
                var createdClass = new ClassArrayCreationTask(fieldsCount);

                var structAverageTime = benchmark
                    .MeasureDurationInMs(createdStruct, repetitionsCount);
                var classAverageTime = benchmark
                    .MeasureDurationInMs(createdClass, repetitionsCount);

                classesTimes.Add(new ExperimentResult(fieldsCount, classAverageTime));
                structuresTimes.Add(new ExperimentResult(fieldsCount, structAverageTime));
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var fieldsCount in Constants.FieldCounts)
            {
                var createdStruct = new MethodCallWithStructArgumentTask(fieldsCount);
                var createdClass = new MethodCallWithClassArgumentTask(fieldsCount);

                var structAverageTime = benchmark
                    .MeasureDurationInMs(createdStruct, repetitionsCount);
                var classAverageTime = benchmark
                    .MeasureDurationInMs(createdClass, repetitionsCount);

                classesTimes.Add(new ExperimentResult(fieldsCount, classAverageTime));
                structuresTimes.Add(new ExperimentResult(fieldsCount, structAverageTime));
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}