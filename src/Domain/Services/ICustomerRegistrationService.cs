using System.Threading.Tasks;
using Common.Dtos;
using Domain.Models;

namespace Domain.Services
{
    public interface ICustomerRegistrationService : IDomainService
    {
        Task<Result<int>> RegisterAsync(Customer customer);
    }
}
