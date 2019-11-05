using System;
using Domain.Models;
using Xunit;

namespace Domain.Tests.Models
{
    public class EmailTests
    {
        #region Construction

        [Fact]
        public void Construction_WithValidEmail_SetsValue()
        {
            //arrange
            var email = "email@test.com";

            //act
            var emailVo = new Email(email);

            //assert
            Assert.Equal(email, emailVo.Value);
        }

        [Fact]
        public void Construction_WithNullEmail_ThrowsArgumentNullException()
        {
            //arrange - act - assert
            var exception = Assert.Throws<ArgumentNullException>(delegate
            {
                new Email(null);
            });

            Assert.Equal("Email cannot be null\r\nParameter name: email", exception.Message);
        }

        [Theory]
        [InlineData("notvalidemail")]
        [InlineData("")]
        [InlineData(" ")]
        public void Construction_WithNotValidEmail_ThrowsArgumentException(string email)
        {
            //arrange - act - assert
            var exception = Assert.Throws<ArgumentException>(delegate
            {
                new Email(email);
            });

            Assert.Equal("Not valid email\r\nParameter name: email", exception.Message);
        }

        #endregion

        #region Equality

        [Fact]
        public void Email_DeterminesEquality_BasedOnAllItsProperties()
        {
            //arrange
            var email = "email@test.com";

            //act
            var email1 = new Email(email);
            var email2 = new Email(email);

            //assert
            Assert.Equal(email1, email2);
        }

        #endregion
    }
}
