namespace CodeAnalyzer.Tests
{
    public class AlwaysTrueTests
    {
        [Fact]
        public void TestWithInvalidName()
        {
            Assert.True(true);
        }

        [Fact]
        public void Test_WithValidName_ReturnsTrue()
        {
            Assert.True(true);
        }
    }
}