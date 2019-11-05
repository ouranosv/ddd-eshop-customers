using System;
using Domain.Models;
using Domain.Repositories;
using Domain.Services.Specifications;
using Moq;
using Xunit;

namespace Domain.Tests.Services.Specifications
{
    public class EmailNotUsedSpecificationTests
    {
        #region IsSatisfiedByAsync

        [Fact]
        public async void IsSatisfiedByAsync_WithEmailThatIsAlreadyInUse_ReturnsFalse()
        {
            //arrange
            var customer = GetCustomer();
            var email = customer.Email;

            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(t => t.FindByEmailAsync(email.Value)).ReturnsAsync(customer);

            var emailNotUsedSpecification = new EmailNotUsedSpecification(customerRepository.Object);

            //act
            var actual = await emailNotUsedSpecification.IsSatisfiedByAsync(email);

            //assert
            Assert.False(actual);
        }

        [Fact]
        public async void IsSatisfiedByAsync_WithEmailThatIsNotInUse_ReturnsTrue()
        {
            //arrange
            var email = new Email("email@test.com");
            Customer customer = null;

            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(t => t.FindByEmailAsync(email.Value)).ReturnsAsync(customer);

            var emailNotUsedSpecification = new EmailNotUsedSpecification(customerRepository.Object);

            //act
            var actual = await emailNotUsedSpecification.IsSatisfiedByAsync(email);

            //assert
            Assert.True(actual);
        }

        #endregion

        #region SampleData

        private Customer GetCustomer()
        {
            var id = new CustomerId(Guid.NewGuid());
            var name = new Name("first", "last");
            var email = new Email("email@test.com");

            return new Customer(id, name, email);
        }

        #endregion
    }
}
