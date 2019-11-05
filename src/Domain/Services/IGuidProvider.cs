using System;

namespace Domain.Services
{
    public interface IGuidProvider : IDomainService
    {
        Guid NewGuid();
    }
}
