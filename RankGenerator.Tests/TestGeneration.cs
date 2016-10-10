using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rank_Generator.Classes;
using static Rank_Generator.RankGenerator;

namespace RankGenerator.Tests
{
    [TestClass]
    public class TestGeneration
    {
        private Rank_Generator.RankGenerator _testGenerator;

        [TestInitialize]
        public void MakeGenerator()
        {
            _testGenerator = Initialize(@"..\..\..\RankGenerator.Examples\Sample Data\politics.json");
        }

        [TestMethod]
        public void NextRank_IsSuccessful()
        {
            // Dummy Rank without props
            var lowestMiscRank = new Rank(
                new Word { Phrase = "Delegate" }, // Title
                new Word { Phrase = "Acting" } // Prefix
            );

            Assert.IsTrue(_testGenerator.NextRank().Equals(lowestMiscRank));
        }
    }
}
