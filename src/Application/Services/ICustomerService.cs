using System.Threading.Tasks;
using Application.Dtos;
using Common.Dtos;

namespace Application.Services
{
    public interface ICustomerService : IApplicationService
    {
        Task<Result<RegisteredCustomerDto>> RegisterAsync(RegisterCustomerDto customerDto);
    }
}
