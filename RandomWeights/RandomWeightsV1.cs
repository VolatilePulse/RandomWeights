#nullable enable

using System.Linq;

namespace RandomWeights
{
    public static class RandomWeightsV1
    {
        public static class RandomWeightsUtils
        {
            public record WeightAndIndices(int Weight, int[] Indices)
            {
                public override string ToString() =>
                    $"Weight = {Weight}, Indices = {string.Join(',', Indices)}";
            };

            public static WeightAndIndices CalculateIndices(int targetWeight, int[] weights)
            {
                var bestWeight = 0;
                var bestState = System.Array.Empty<int>();

                var state = new int[] { 0 };
                var size = weights.Length;

                var fullState = Enumerable.Range(0, size).ToArray();
                var fullWeight = SumState(fullState, weights);

                // Bail out if we can't reach the target weight
                if (targetWeight >= fullWeight)
                    return new WeightAndIndices(fullWeight, fullState);

                while (state.Any())
                {
                    var totalWeight = SumState(state, weights);

                    if (totalWeight > targetWeight)
                        state = NextStepSnip(state, size);
                    else if (totalWeight == targetWeight)
                        return new WeightAndIndices(totalWeight, state);
                    else
                    {
                        if (totalWeight > bestWeight)
                        {
                            bestWeight = totalWeight;
                            bestState = state;
                        }

                        state = NextStep(state, size);
                    }
                }

                return new WeightAndIndices(bestWeight, bestState);
            }

            public static int SumState(int[] state, int[] values) => state.Select(s => values[s]).Sum();

            public static int[] NextStepSnip(int[] state, int size)
            {
                if (!state.Any()) return state;

                // Back track to the next weight
                if (state[^1] == size - 1) return NextStepSnip(state[..^1], size);

                // Lateral step
                return state[..^1].Add(state[^1] + 1);
            }

            public static int[] NextStep(int[] state, int size)
            {
                if (!state.Any()) return state;

                var i = state[^1];

                if (i == size - 1)
                {
                    state = state[..^1];
                    return NextStepSnip(state, size);
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