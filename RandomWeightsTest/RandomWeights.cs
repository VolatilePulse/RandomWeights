using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RandomWeights.RandomWeightsV1;

namespace RandomWeights
{
    [TestClass]
    public class MainMethod
    {
        private readonly int[] TestNumbers = { 1, 2, 3 };

        [TestMethod]
        public void EmptyArray()
        {
            var expected = TestNumbers;
            var actual = TestNumbers;
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}