#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomWeights
{
    internal class Program
    {
        private static void Main()
        {
            var targetWeight = 100_430; // 80 Iterations
            var weights = new List<int>();
            weights.AddRange(Enumerable.Repeat(1000, 2));
            weights.AddRange(Enumerable.Repeat(2000, 2));
            weights.AddRange(Enumerable.Repeat(4000, 7));
            weights.AddRange(Enumerable.Repeat(5000, 2));
            weights.AddRange(Enumerable.Repeat(6000, 7));
            weights.AddRange(Enumerable.Repeat(8000, 2));
            var testWeights = weights.OrderByDescending(i => i).ToArray();

            var result = RandomWeightsV2.RandomWeightsUtils.CalculateIndices(targetWeight, testWeights);

            Console.WriteLine(result);
            Console.WriteLine();

            var slowestTime = new TimeSpan(0);
            var slowestWeights = Array.Empty<int>();
            var slowestTargetWeight = 0;

            foreach (var i in Enumerable.Range(0, testWeights.Sum()))
            {
                if (i % 10_000 == 0) Console.WriteLine($"Iteration {i}");
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var results = RandomWeightsV2.RandomWeightsUtils.CalculateIndices(i, testWeights);
                sw.Stop();

                if (sw.Elapsed > slowestTime)
                {
                    slowestTime = sw.Elapsed;
                    slowestWeights = results.Indices.Select(i => testWeights[i]).ToArray();
                    slowestTargetWeight = i;
                }
            }

            Console.WriteLine(slowestTime);
            Console.WriteLine(string.Join(',', slowestWeights));
            Console.WriteLine(slowestTargetWeight);
        }
    }
}