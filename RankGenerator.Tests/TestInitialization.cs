using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Rank_Generator.RankGenerator;

namespace RankGenerator.Tests
{
    [TestClass]
    public class TestInitialization
    {
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void Initialize_ThrowsException_InvalidPath()
        {
            Initialize(@"Path\That\Doesn't\Exist");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Initialize_ThrowsException_MissingFile()
        {
            Initialize("Nonexistent.json");
        }
    }
}
