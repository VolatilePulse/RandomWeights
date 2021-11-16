using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RandomWeights.RandomWeightsV1.RandomWeightsUtils;

namespace RandomWeightsUtils
{
    [TestClass]
    public class CalculateIndices
    {
        private readonly int[] TestWeights = { 4, 2, 2, 1 };

        [TestMethod]
        public void TargetWeight_1()
        {
            var expected = new WeightAndIndices(1, new int[] { 3 });
            var actual = CalculateIndices(1, TestWeights);

            Assert.AreEqual(expected.Weight, actual.Weight);
            CollectionAssert.AreEqual(expected.Indices, actual.Indices);
        }

        [TestMethod]
        public void TargetWeight_2()
        {
            var expected = new WeightAndIndices(2, new int[] { 1 });
            var actual = CalculateIndices(2, TestWeights);

            Assert.AreEqual(expected.Weight, actual.Weight);
            CollectionAssert.AreEqual(expected.Indices, actual.Indices);
        }

        [TestMethod]
        public void TargetWeight_3()
        {
            var expected = new WeightAndIndices(3, new int[] { 1, 3 });
            var actual = CalculateIndices(3, TestWeights);

            Assert.AreEqual(expected.Weight, actual.Weight);
            CollectionAssert.AreEqual(expected.Indices, actual.Indices);
        }

        [TestMethod]
        public void TargetWeight_8()
        {
            var expected = new WeightAndIndices(8, new int[] { 0, 1, 2 });
            var actual = CalculateIndices(8, TestWeights);

            Assert.AreEqual(expected.Weight, actual.Weight);
            CollectionAssert.AreEqual(expected.Indices, actual.Indices);
        }

        [TestMethod]
        public void TargetWeight_9()
        {
            var expected = new WeightAndIndices(9, new int[] { 0, 1, 2, 3 });
            var actual = CalculateIndices(9, TestWeights);

            Assert.AreEqual(expected.Weight, actual.Weight);
            CollectionAssert.AreEqual(expected.Indices, actual.Indices);
        }

        [TestMethod]
        public void TargetWeight_10()
        {
            var expected = new WeightAndIndices(9, new int[] { 0, 1, 2, 3 });
            var actual = CalculateIndices(10, TestWeights);

            Assert.AreEqual(expected.Weight, actual.Weight);
            CollectionAssert.AreEqual(expected.Indices, actual.Indices);
        }
    }

    [TestClass]
    public class SumStateTest
    {
        private readonly int[] TestNumbers = { 1, 2, 3 };

        [TestMethod]
        public void EmptyArray()
        {
            var expected = 0;
            var actual = SumState(System.Array.Empty<int>(), TestNumbers);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstElement()
        {
            var expected = 1;
            var actual = SumState(new int[] { 0 }, TestNumbers);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstTwoElements()
        {
            var expected = 3;
            var actual = SumState(new int[] { 0, 1 }, TestNumbers);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AllElements()
        {
            var expected = 6;
            var actual = SumState(new int[] { 0, 1, 2 }, TestNumbers);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AllElementsReversed()
        {
            var expected = 6;
            var actual = SumState(new int[] { 2, 1, 0 }, TestNumbers);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastElement()
        {
            var expected = 3;
            var actual = SumState(new int[] { 2 }, TestNumbers);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class NextStepSnipTest
    {
        [TestMethod]
        public void EmptyArray()
        {
            var expected = System.Array.Empty<int>();
            var actual = NextStepSnip(System.Array.Empty<int>(), 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SingleItem_LateralSnip()
        {
            var expected = new int[] { 1 };
            var actual = NextStepSnip(new int[] { 0 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TwoSequentialItems_LateralSnip()
        {
            var expected = new int[] { 0, 2 };
            var actual = NextStepSnip(new int[] { 0, 1 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstAndLast_ReverseStep()
        {
            var expected = new int[] { 1 };
            var actual = NextStepSnip(new int[] { 0, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AllElements_ReverseStep()
        {
            var expected = new int[] { 0, 1, 3 };
            var actual = NextStepSnip(new int[] { 0, 1, 2, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastElement_StepOut()
        {
            var expected = System.Array.Empty<int>();
            var actual = NextStepSnip(new int[] { 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class NextStepTest
    {
        [TestMethod]
        public void From_0_To_01()
        {
            var expected = new int[] { 0, 1 };
            var actual = NextStep(new int[] { 0 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_01_To_012()
        {
            var expected = new int[] { 0, 1, 2 };
            var actual = NextStep(new int[] { 0, 1 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_012_To_0123()
        {
            var expected = new int[] { 0, 1, 2, 3 };
            var actual = NextStep(new int[] { 0, 1, 2 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_0123_To_013()
        {
            var expected = new int[] { 0, 1, 3 };
            var actual = NextStep(new int[] { 0, 1, 2, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_013_To_02()
        {
            var expected = new int[] { 0, 2 };
            var actual = NextStep(new int[] { 0, 1, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_02_To_023()
        {
            var expected = new int[] { 0, 2, 3 };
            var actual = NextStep(new int[] { 0, 2 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_023_To_03()
        {
            var expected = new int[] { 0, 3 };
            var actual = NextStep(new int[] { 0, 2, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_03_To_1()
        {
            var expected = new int[] { 1 };
            var actual = NextStep(new int[] { 0, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_1_To_12()
        {
            var expected = new int[] { 1, 2 };
            var actual = NextStep(new int[] { 1 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_12_To_123()
        {
            var expected = new int[] { 1, 2, 3 };
            var actual = NextStep(new int[] { 1, 2 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_123_To_13()
        {
            var expected = new int[] { 1, 3 };
            var actual = NextStep(new int[] { 1, 2, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_13_To_2()
        {
            var expected = new int[] { 2 };
            var actual = NextStep(new int[] { 1, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_2_To_23()
        {
            var expected = new int[] { 2, 3 };
            var actual = NextStep(new int[] { 2 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_23_To_3()
        {
            var expected = new int[] { 3 };
            var actual = NextStep(new int[] { 2, 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_3_To_Empty()
        {
            var expected = System.Array.Empty<int>();
            var actual = NextStep(new int[] { 3 }, 4);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}