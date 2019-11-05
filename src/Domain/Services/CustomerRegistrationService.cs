using System.Threading.Tasks;
using Common.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Services.Specifications;

namespace Domain.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailNotUsedSpecification _emailNotUsedSpecification;

        public CustomerRegistrationService(ICustomerRepository customerRepository, IEmailNotUsedSpecification emailNotUsedSpecification)
        {
            _customerRepository = customerRepository;
            _emailNotUsedSpecification = emailNotUsedSpecification;
        }

        public async Task<Result<int>> RegisterAsync(Customer customer)
        {
            var emailIsUsed = !await _emailNotUsedSpecification.IsSatisfiedByAsync(customer.Email);

            if (emailIsUsed)
                return Result<int>.Fail("Email is already in use");

            return Result<int>.Success(await _customerRepository.InsertAsync(customer));
        }
    }
}
