namespace CodeAnalyzer.Tests
{
    public class AlwaysTrueTests
    {
        [Fact]
        public void Fact_WithValidName_ReturnsTrue()
        {
            Assert.True(true);
        }

        [Fact]
        public void FactWithInvalidNameNoUnderscores()
        {
            Assert.True(true);
        }

        [Fact]
        public void FactWithInvalidName_NotEnoughUnderscores()
        {
            Assert.True(true);
        }

        [Fact]
        public void FactWithInvalidNameUnderscoreAtEnd_()
        {
            Assert.True(true);
        }

        [Fact]
        public void _FactWithInvalidNameUnderscoreAtStart()
        {
            Assert.True(true);
        }

        [Fact]
        public void _FactWithInvalidNameUnderscoreAtStartAndEnd_()
        {
            Assert.True(true);
        }

        [Fact]
        public void Fact_With_Invalid_Name_Too_Many_Underscores()
        {
            Assert.True(true);
        }

        [Theory]
        [InlineData(true)]
        public void Theory_WithValidName_ReturnsTrue(bool value)
        {
            Assert.True(value);
        }

        [Theory]
        [InlineData(true)]
        public void TheoryWithInvalidNameNoUnderscores(bool value)
        {
            Assert.True(value);
        }

        [Theory]
        [InlineData(true)]
        public void TheoryWithInvalidName_NotEnoughUnderscores(bool value)
        {
            Assert.True(value);
        }

        [Theory]
        [InlineData(true)]
        public void TheoryWithInvalidNameUnderscoreAtEnd_(bool value)
        {
            Assert.True(value);
        }

        [Theory]
        [InlineData(true)]
        public void _TheoryWithInvalidNameUnderscoreAtStart(bool value)
        {
            Assert.True(value);
        }

        [Theory]
        [InlineData(true)]
        public void _TheoryWithInvalidNameUnderscoreAtStartAndEnd_(bool value)
        {
            Assert.True(value);
        }

        [Theory]
        [InlineData(true)]
        public void Theory_With_Invalid_Name_Too_Many_Underscores(bool value)
        {
            Assert.True(value);
        }
    }
}