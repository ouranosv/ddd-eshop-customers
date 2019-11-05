using System;

namespace Domain.Models.Base
{
    public abstract class Guidentity : ValueObject<Guidentity>, IIdentity<Guid>
    {
        public Guid Value { get; private set; }

        public Guidentity(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", "id");

            Value = id;
        }
    }
}
