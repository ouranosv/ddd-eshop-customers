using System;
using Common.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Domain.Services.Specifications;
using ExpectedObjects;
using Moq;
using Xunit;

namespace Domain.Tests.Services
{
    public class CustomerRegistrationServiceTests
    {
        #region RegisterAsync

        [Fact]
        public async void Register_WithCustomer_InsertsCustomerInDBAndReturnsSuccessResult()
        {
            //arrange
            var customer = GetCustomer();
            var expected = Result<int>.Success(1);

            var customerRepository = new Mock<ICustomerRepository>();
            var emailNotUsedSpecification = new Mock<IEmailNotUsedSpecification>();

            customerRepository.Setup(t => t.InsertAsync(customer)).ReturnsAsync(1);
            emailNotUsedSpecification.Setup(t => t.IsSatisfiedByAsync(customer.Email)).ReturnsAsync(true);

            var customerRegistrationService = new CustomerRegistrationService(customerRepository.Object, emailNotUsedSpecification.Object);

            //act
            var actual = await customerRegistrationService.RegisterAsync(customer);

            //assert
            expected.ToExpectedObject().ShouldEqual(actual);
            customerRepository.Verify(t => t.InsertAsync(customer), Times.Once);
        }

        [Fact]
        public async void RegisterAsync_WithCustomerAndEmailThatIsAlreadyInUse_ReturnsFailureResult()
        {
            //arrange
            var customer = GetCustomer();
            var expected = Result<int>.Fail("Email is already in use");

            var customerRepository = new Mock<ICustomerRepository>();
            var emailNotUsedSpecification = new Mock<IEmailNotUsedSpecification>();

            customerRepository.Setup(t => t.InsertAsync(customer)).ReturnsAsync(1);
            emailNotUsedSpecification.Setup(t => t.IsSatisfiedByAsync(customer.Email)).ReturnsAsync(false);

            var customerRegistrationService = new CustomerRegistrationService(customerRepository.Object, emailNotUsedSpecification.Object);

            //act 
            var actual = await customerRegistrationService.RegisterAsync(customer);

            //assert
            expected.ToExpectedObject().ShouldEqual(actual);
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
