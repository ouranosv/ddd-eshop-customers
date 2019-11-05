using DataAccess.UnitsOfWork;

namespace DataAccess.MsSql.Repositories
{
    public abstract class Repository
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
