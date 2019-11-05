using Application.Dtos;
using Application.Services;
using Common.Dtos;
using DataAccess.UnitsOfWork;
using ExpectedObjects;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Messages.CustomerRegistered;
using Xunit;

namespace WebApi.Tests.Controllers
{
    public class CustomerControllerTests
    {
        #region RegisterAsync

        [Fact]
        public async void RegisterAsync_WhenEverythingOk_ReturnsSuccessMessage()
        {
            //arrange
            var expected = new SuccessMessage();

            var customer = new RegisterCustomerDto
            {
                FirstName = "first",
                LastName = "last",
                Email = "email@test.com"
            };

            var registeredCustomerResult = Result<RegisteredCustomerDto>.Success(new RegisteredCustomerDto
            {
                Id = "08062E33-889E-472A-870A-650F3F018DE7",
                FirstName = "first",
                LastName = "last",
                Email = "email@test.com"
            });

            var customerRegisteredEvent = new CustomerRegisteredEvent
            {
                CustomerId = "08062E33-889E-472A-870A-650F3F018DE7",
                CustomerFirstName = "first",
                CustomerLastName = "last",
                CustomerEmail = "email@test.com"
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            var customerService = new Mock<ICustomerService>();
            var bus = new Mock<IBus>();

            customerService.Setup(t => t.RegisterAsync(
                It.Is<RegisterCustomerDto>(a => customer.ToExpectedObject().Equals(a))
            )).ReturnsAsync(registeredCustomerResult);

            var customerController = new CustomerController(customerService.Object, bus.Object, unitOfWork.Object);

            //act
            var actual = await customerController.RegisterAsync(customer);

            //assert
            var okObjectResult = actual as OkObjectResult;

            Assert.NotNull(okObjectResult);
            expected.ToExpectedObject().ShouldEqual(okObjectResult.Value);

            unitOfWork.Verify(t => t.SaveChanges(), Times.Once);

            bus.Verify(t => t.Publish(
                It.Is<CustomerRegisteredEvent>(a => customerRegisteredEvent.ToExpectedObject().Equals(a)),
                default
            ), Times.Once);
        }

        [Fact]
        public async void RegisterAsync_WhenThingsFail_ReturnsErrorMessage()
        {
            //arrange
            var expected = new ErrorMessage("Email is already in use");

            var customer = new RegisterCustomerDto
            {
                FirstName = "first",
                LastName = "last",
                Email = "email@test.com"
            };

            var registeredCustomerResult = Result<RegisteredCustomerDto>.Fail("Email is already in use");

            var unitOfWork = new Mock<IUnitOfWork>();
            var customerService = new Mock<ICustomerService>();
            var bus = new Mock<IBus>();

            customerService.Setup(t => t.RegisterAsync(
                It.Is<RegisterCustomerDto>(a => customer.ToExpectedObject().Equals(a))
            )).ReturnsAsync(registeredCustomerResult);

            var customerController = new CustomerController(customerService.Object, bus.Object, unitOfWork.Object);

            //act
            var actual = await customerController.RegisterAsync(customer);

            //assert
            var okObjectResult = actual as OkObjectResult;

            Assert.NotNull(okObjectResult);
            expected.ToExpectedObject().ShouldEqual(okObjectResult.Value);

            unitOfWork.Verify(t => t.SaveChanges(), Times.Exactly(0));

            bus.Verify(t => t.Publish(
                It.IsAny<CustomerRegisteredEvent>(),
                default
            ), Times.Exactly(0));
        }

        #endregion
    }
}
