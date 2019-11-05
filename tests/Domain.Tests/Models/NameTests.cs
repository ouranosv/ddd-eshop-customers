using System;
using Domain.Models;
using Xunit;

namespace Domain.Tests.Models
{
    public class NameTests
    {
        #region Construction

        [Fact]
        public void Construction_WithValidFirstAndLastName_SetsValues()
        {
            //arrange
            var first = "first";
            var last = "last";

            //act
            var name = new Name(first, last);

            //assert
            Assert.Equal(first, name.First);
            Assert.Equal(last, name.Last);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Construction_WithNullOrWhitespaceFirstName_ThrowsArgumentNullException(string first)
        {
            //arrange
            var last = "last";

            //act - assert
            var exception = Assert.Throws<ArgumentNullException>(delegate
            {
                new Name(first, last);
            });

            Assert.Equal("Firstname cannot be null or whitespace\r\nParameter name: first", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Construction_WithNullOrWhitespaceLastName_ThrowsArgumentNullException(string last)
        {
            //arrange
            var first = "first";

            //act - assert
            var exception = Assert.Throws<ArgumentNullException>(delegate
            {
                new Name(first, last);
            });

            Assert.Equal("Lastname cannot be null or whitespace\r\nParameter name: last", exception.Message);
        }

        #endregion

        #region Equality

        [Fact]
        public void Name_DeterminesEquality_BasedOnAllItsProperties()
        {
            //arrange
            var first = "first";
            var last = "last";

            //act
            var name1 = new Name(first, last);
            var name2 = new Name(first, last);

            //assert
            Assert.Equal(name1, name2);
        }

        #endregion
    }
}
