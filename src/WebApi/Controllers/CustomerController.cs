using System.Threading.Tasks;
using Application.Dtos;
using Application.Services;
using Common.Dtos;
using DataAccess.UnitsOfWork;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebApi.Messages.CustomerRegistered;

namespace WebApi.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IBus _bus;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(ICustomerService customerService, IBus bus, IUnitOfWork unitOfWork)
        {
            _customerService = customerService;
            _bus = bus;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Registers a customer
        /// </summary>
        [HttpPost("customers/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCustomerDto customerDto)
        {
            var result = await _customerService.RegisterAsync(customerDto);

            if (result.IsFailure)
                return Ok(new ErrorMessage(result.Error));

            _unitOfWork.SaveChanges();

            _bus.Publish(new CustomerRegisteredEvent
            {
                CustomerId = result.Value.Id,
                CustomerFirstName = result.Value.FirstName,
                CustomerLastName = result.Value.LastName,
                CustomerEmail = result.Value.Email
            }).Wait();

            return Ok(new SuccessMessage());
        }
    }
}
