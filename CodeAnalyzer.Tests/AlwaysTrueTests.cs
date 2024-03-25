#pragma warning disable IDE1006 // Naming Styles
namespace CodeAnalyzer.Tests
{
    public class AlwaysTrueTests
    {
        [Fact]
        public void Fact_With_ValidName()
        {
        }

        [Fact]
        public void FactWithInvalidNameNoUnderscores()
        {
        }

        [Fact]
        public void FactWithInvalidName_NotEnoughUnderscores()
        {
        }

        [Fact]
        public void FactWithInvalidNameUnderscoreAtEnd_()
        {
        }

        [Fact]
        public void _FactWithInvalidNameUnderscoreAtStart()
        {
        }

        [Fact]
        public void _FactWithInvalidNameUnderscoreAtStartAndEnd_()
        {
        }

        [Fact]
        public void Fact_With_Invalid_Name_Too_Many_Underscores()
        {
        }

        [Theory]
        [InlineData(true)]
        public void Theory_WithValidName_ReturnsTrue(bool value) => Assert.True(value);

        [Theory]
        [InlineData(true)]
        public void TheoryWithInvalidNameNoUnderscores(bool value) => Assert.True(value);

        [Theory]
        [InlineData(true)]
        public void TheoryWithInvalidName_NotEnoughUnderscores(bool value) => Assert.True(value);

        [Theory]
        [InlineData(true)]
        public void TheoryWithInvalidNameUnderscoreAtEnd_(bool value) => Assert.True(value);

        [Theory]
        [InlineData(true)]
        public void _TheoryWithInvalidNameUnderscoreAtStart(bool value) => Assert.True(value);

        [Theory]
        [InlineData(true)]
        public void _TheoryWithInvalidNameUnderscoreAtStartAndEnd_(bool value) => Assert.True(value);

        [Theory]
        [InlineData(true)]
        public void Theory_With_Invalid_Name_Too_Many_Underscores(bool value) => Assert.True(value);
    }
}