using System;
using System.Collections.Generic;

namespace StructBenchmarking;

public abstract class Factory
{
	public abstract ITask CreateClassTask(int count);

	public abstract ITask CreateStructTask(int count);
}

public class ArrayFactory : Factory
{
	public override ITask CreateClassTask(int count)
	{
		return new ClassArrayCreationTask(count);
	}

	public override ITask CreateStructTask(int count)
	{
		return new StructArrayCreationTask(count);
	}
}

public class MethodFactory : Factory
{
	public override ITask CreateClassTask(int count)
	{
		return new MethodCallWithClassArgumentTask(count);
	}

	public override ITask CreateStructTask(int count)
	{
		return new MethodCallWithStructArgumentTask(count);
	}
}

public class Experiments
{
	public static ChartData BuildChartData(
		IBenchmark benchmark, int repetitionsCount, Factory factory, string title)
	{
		var classesTimes = new List<ExperimentResult>();
		var structuresTimes = new List<ExperimentResult>();

		foreach (var i in Constants.FieldCounts)
		{
			var classArray = factory.CreateClassTask(i);
			double classTime = benchmark.MeasureDurationInMs(classArray, repetitionsCount);
			ExperimentResult classResult = new ExperimentResult(i, classTime);
			classesTimes.Add(classResult);
			
			var structArray = factory.CreateStructTask(i);
			double structTime = benchmark.MeasureDurationInMs(structArray, repetitionsCount);
			ExperimentResult structResult = new ExperimentResult(i, structTime);
			structuresTimes.Add(structResult);
		}

		return new ChartData
		{
			Title = title,
			ClassPoints = classesTimes,
			StructPoints = structuresTimes,
		};
	}
}