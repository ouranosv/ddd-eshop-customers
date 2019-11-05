using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Services.Specifications
{
    public class EmailNotUsedSpecification : IEmailNotUsedSpecification
    {
        private readonly ICustomerRepository _customerRepository;

        public EmailNotUsedSpecification(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> IsSatisfiedByAsync(Email email)
        {
            var customer = await _customerRepository.FindByEmailAsync(email.Value);

            if (customer == null) return true;

            return false;
        }
    }
}
