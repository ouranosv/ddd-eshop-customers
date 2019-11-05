using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface ICustomerRepository : IRepository
    {
        Task<int> InsertAsync(Customer customer);
        Task<Customer> FindByEmailAsync(string email);
    }
}
