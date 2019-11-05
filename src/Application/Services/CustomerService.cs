using System.Threading.Tasks;
using Application.Dtos;
using Common.Dtos;
using Domain.Models;
using Domain.Services;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IGuidProvider _guidProvider;

        public CustomerService(ICustomerRegistrationService customerRegistrationService, IGuidProvider guidProvider)
        {
            _customerRegistrationService = customerRegistrationService;
            _guidProvider = guidProvider;
        }

        public async Task<Result<RegisteredCustomerDto>> RegisterAsync(RegisterCustomerDto customerDto)
        {
            var guid = _guidProvider.NewGuid();
            var customerId = new CustomerId(guid);
            var name = new Name(customerDto.FirstName, customerDto.LastName);
            var email = new Email(customerDto.Email);
            var customer = new Customer(customerId, name, email);
            var result = await _customerRegistrationService.RegisterAsync(customer);

            if (result.IsFailure)
            {
                return Result<RegisteredCustomerDto>.Fail(result.Error);
            }

            var registeredCustomerDto = new RegisteredCustomerDto
            {
                Id = customer.Id.Value.ToString(),
                FirstName = customer.Name.First,
                LastName = customer.Name.Last,
                Email = customer.Email.Value
            };

            return Result<RegisteredCustomerDto>.Success(registeredCustomerDto);
        }
    }
}
