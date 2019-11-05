using System;
using Domain.Models;
using Xunit;

namespace Domain.Tests.Models
{
    public class CustomerTests
    {
        #region Construction

        [Fact]
        public void Construction_WithValidData_SetsPropertiesCorrectly()
        {
            //arrange
            var id = new CustomerId(Guid.NewGuid());
            var name = new Name("fist name", "last name");
            var email = new Email("test@email.com");

            //act
            var customer = new Customer(id, name, email);

            //assert
            Assert.Equal(id, customer.Id);
            Assert.Equal(name, customer.Name);
            Assert.Equal(email, customer.Email);
        }

        [Fact]
        public void Construction_WithNullCustomerId_ThrowsArgumentNullException()
        {
            //arrange
            var name = new Name("fist name", "last name");
            var email = new Email("test@email.com");

            //act - assert
            var exception = Assert.Throws<ArgumentNullException>(delegate
            {
                new Customer(null, name, email);
            });

            Assert.Equal("Id cannot be null\r\nParameter name: id", exception.Message);
        }

        [Fact]
        public void Construction_WithNullName_ThrowsArgumentNullException()
        {
            //arrange
            var id = new CustomerId(Guid.NewGuid());
            var email = new Email("test@email.com");

            //act - assert
            var exception = Assert.Throws<ArgumentNullException>(delegate
            {
                new Customer(id, null, email);
            });

            Assert.Equal("Name cannot be null\r\nParameter name: name", exception.Message);
        }

        [Fact]
        public void Construction_WithNullEmail_ThrowsArgumentNullException()
        {
            //arrange
            var id = new CustomerId(Guid.NewGuid());
            var name = new Name("fist name", "last name");

            //act - assert
            var exception = Assert.Throws<ArgumentNullException>(delegate
            {
                new Customer(id, name, null);
            });

            Assert.Equal("Email cannot be null\r\nParameter name: email", exception.Message);
        }

        #endregion

        #region Equality

        [Fact]
        public void Customer_DeterminesEquality_BasedOnItsId()
        {
            //arrange
            var id = new CustomerId(Guid.NewGuid());
            var name1 = new Name("fist name1", "last name1");
            var name2 = new Name("fist name2", "last name2");
            var email1 = new Email("test@email1.com");
            var email2 = new Email("test@email2.com");

            //act
            var customer1 = new Customer(id, name1, email1);
            var customer2 = new Customer(id, name2, email2);

            //assert
            Assert.Equal(customer1, customer2);
        }

        #endregion
    }
}
