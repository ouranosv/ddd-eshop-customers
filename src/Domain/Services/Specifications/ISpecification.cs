using System.Threading.Tasks;

namespace Domain.Services.Specifications
{
    public interface ISpecification<T> : IDomainService
    {
        Task<bool> IsSatisfiedByAsync(T candidate);
    }
}
