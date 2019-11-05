using System;

namespace Domain.Models.Base
{
    public abstract class Entity<T>
    {
        public IIdentity<T> Id { get; private set; }

        public Entity(IIdentity<T> id)
        {
            if (id == null)
                throw new ArgumentNullException("id", "Id cannot be null");

            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Entity<T>)obj;

            return SameIdentityAs(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private bool SameIdentityAs(Entity<T> other)
        {
            return other != null && GetType() == other.GetType() && Id == other.Id;
        }
    }
}
