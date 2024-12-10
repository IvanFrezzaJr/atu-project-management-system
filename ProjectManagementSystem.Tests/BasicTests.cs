using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectManagementSystem.Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void TestIfTestsRun()
        {
            // Arrange
            int expected = 5;
            int actual = 5;

            // Assert
            Assert.AreEqual(expected, actual, "The values ​​are not the same!");
        }
    }
}
