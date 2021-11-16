#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace RandomWeights
{
    public static class RandomWeightsV2
    {
        public static class RandomWeights
        {
            public class WeightVisual
            {
                public Weight Weight;

                public WeightVisual(int capacity, int stackOrder, bool yoke) => Weight = new Weight(capacity, stackOrder, yoke);
            }

            public class Weight
            {
                public decimal Capacity;
                public int StackOrder;
                public bool Yoke;

                public Weight(int capacity, int stackOrder, bool yoke)
                {
                    Capacity = capacity;
                    StackOrder = stackOrder;
                    Yoke = yoke;
                }
            }

            public static IEnumerable<WeightVisual> RandomStackWeights(IEnumerable<WeightVisual> stackWeights, decimal targetWeight)
            {
                var yokes = stackWeights.Where(w => w.Weight.Yoke);
                var yokeWeight = yokes.Sum(y => y.Weight.Capacity);

                if (yokeWeight > targetWeight)
                    throw new System.ArgumentException($"{nameof(targetWeight)} is lower than the weight of the yoke(s)", nameof(targetWeight));

                var adjustedTargetWeight = decimal.ToInt32(targetWeight - yokes.Sum(y => y.Weight.Capacity));
                var nonYokeWeights = stackWeights.Except(yokes).Select(w => decimal.ToInt32(w.Weight.Capacity)).ToArray();

                var weightsToLoad = CalculateOptimalWeights(adjustedTargetWeight, nonYokeWeights);

                return new List<WeightVisual>();
            }

            public static int[] CalculateOptimalWeights(int targetWeight, int[] weights)
            {
                var (weight, indices) = RandomWeightsUtils.CalculateIndices(targetWeight, weights);

                if (weight > targetWeight)
                    throw new System.Exception($"Calculated weight ({weight}) is larger than the target weight ({targetWeight})");

                return indices.Select(i => weights[i]).ToArray();
            }
        }

        public static class RandomWeightsUtils
        {
            public record WeightAndIndices(int Weight, int[] Indices)
            {
                public override string ToString() =>
                    $"Weight = {Weight}, Indices = {string.Join(',', Indices)}";
            }

            public static WeightAndIndices CalculateIndices(int targetWeight, int[] weights)
            {
                var orderedWeights = weights.OrderByDescending(x => x).ToArray();

                var bestWeight = 0;
                var bestState = System.Array.Empty<int>();

                var state = new int[] { 0 };
                var size = orderedWeights.Length;

                var fullState = Enumerable.Range(0, size).ToArray();
                var fullWeight = SumState(fullState, orderedWeights);

                // Bail out if we can't reach the target weight
                if (targetWeight >= fullWeight)
                    return new WeightAndIndices(fullWeight, fullState);

                while (state.Any())
                {
                    var totalWeight = SumState(state, orderedWeights);

                    if (totalWeight > targetWeight)
                        state = NextStepSnip(state, orderedWeights, size);
                    else if (totalWeight == targetWeight)
                        return new WeightAndIndices(totalWeight, state);
                    else
                    {
                        if (totalWeight > bestWeight)
                        {
                            bestWeight = totalWeight;
                            bestState = state;
                        }

                        if (HasSmallerRemainingWeights(orderedWeights, state[^1], targetWeight - bestWeight))
                            state = NextStep(state, orderedWeights, size);
                        else
                            state = NextStepSnip(state, orderedWeights, size);
                    }
                }

                return new WeightAndIndices(bestWeight, bestState);
            }

            public static bool HasSmallerRemainingWeights(int[] weights, int highestIndex, int weightDiff) =>
                weights[highestIndex..^1].Any(w => w < weightDiff);

            public static int SumState(int[] state, int[] values) => state.Select(s => values[s]).Sum();

            public static int[] NextStepSnip(int[] state, int[] weights, int size)
            {
                if (!state.Any()) return state;

                // Back track to the next weight
                if (state[^1] == size - 1) return NextStepSnip(state[..^1], weights, size);

                // Lateral step
                var index = state[^1];
                while (index < size)
                {
                    // Found the next unique weight
                    if (weights[state[^1]] != weights[index])
                        return state[..^1].Add(index);

                    index++;
                }

                // No more unique weights
                return NextStepSnip(state[..^1].Add(index - 1), weights, size);
            }

            public static int[] NextStep(int[] state, int[] weights, int size)
            {
                if (!state.Any()) return state;

                var i = state[^1];

                if (i == size - 1)
                {
                    state = state[..^1];
                    return NextStepSnip(state, weights, size);
                }
                return state.Add(i + 1);
            }
        }

        private static int[] Add(this int[] array, int value)
        {
            var tempArray = new int[array.Length + 1];

            System.Array.Copy(array, tempArray, array.Length);
            tempArray[^1] = value;

            return tempArray;
        }
    }
}