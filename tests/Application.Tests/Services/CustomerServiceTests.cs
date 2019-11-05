using System;
using Application.Dtos;
using Application.Services;
using Common.Dtos;
using Domain.Models;
using Domain.Services;
using ExpectedObjects;
using Moq;
using Xunit;

namespace Application.Tests.Services
{
    public class CustomerServiceTests
    {
        #region RegisterAsync

        [Fact]
        public async void RegisterAsync_WithCustomerDto_ReturnsResultWithAffectedRows()
        {
            //arrange
            var guid = Guid.NewGuid();

            var customerDto = new RegisterCustomerDto
            {
                Email = "email@test.com",
                FirstName = "first",
                LastName = "last"
            };

            var registeredCustomerDto = new RegisteredCustomerDto
            {
                Id = guid.ToString(),
                FirstName = "first",
                LastName = "last",
                Email = "email@test.com"
            };

            var id = new CustomerId(guid);
            var name = new Name("first", "last");
            var email = new Email("email@test.com");
            var customer = new Customer(id, name, email);
            var registrationResult = Result<int>.Success(1);
            var expected = Result<RegisteredCustomerDto>.Success(registeredCustomerDto);

            var customerRegistrationService = new Mock<ICustomerRegistrationService>();
            var guidProvider = new Mock<IGuidProvider>();

            customerRegistrationService.Setup(t => t.RegisterAsync(
                It.Is<Customer>(a => customer.ToExpectedObject().Equals(a))
            )).ReturnsAsync(registrationResult);

            guidProvider.Setup(t => t.NewGuid()).Returns(guid);

            var customerService = new CustomerService(customerRegistrationService.Object, guidProvider.Object);

            //act
            var actual = await customerService.RegisterAsync(customerDto);

            //assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        #endregion
    }
}
