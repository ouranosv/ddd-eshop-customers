using System;
using Domain.Models;
using Xunit;

namespace Domain.Tests.Models
{
    public class CustomerIdTests
    {
        #region Construction

        [Fact]
        public void Construction_WithId_SetsIdValue()
        {
            //arrange
            var id = Guid.NewGuid();

            //act
            var customerId = new CustomerId(id);

            //assert
            Assert.Equal(id, customerId.Value);
        }

        [Fact]
        public void Construction_WithEmptyGuid_ThrowsArgumentException()
        {
            //arrange - act - assert
            var exception = Assert.Throws<ArgumentException>(delegate
            {
                new CustomerId(Guid.Empty);
            });

            Assert.Equal("Id cannot be empty\r\nParameter name: id", exception.Message);
        }

        #endregion

        #region Equality

        [Fact]
        public void CustomerId_DeterminesEquality_BasedOnAllItsProperties()
        {
            //arrange
            var id = Guid.NewGuid();

            //act
            var customerId1 = new CustomerId(id);
            var customerId2 = new CustomerId(id);

            //assert
            Assert.Equal(customerId1, customerId2);
        }

        #endregion
    }
}
