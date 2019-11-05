using System;
using System.Linq;
using System.Reflection;

namespace Domain.Models.Base
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        public bool Equals(T other)
        {
            if ((object)other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            PropertyInfo[] publicProperties = GetType().GetProperties();

            if (publicProperties != null && publicProperties.Any())
            {
                return publicProperties.All(p =>
                {
                    var left = p.GetValue(this, null);
                    var right = p.GetValue(other, null);

                    if (left == null && right == null)
                        return true;
                    else if (left == null)
                        return false;

                    if (typeof(T).IsAssignableFrom(left.GetType()))
                    {
                        return ReferenceEquals(left, right);
                    }
                    else
                        return left.Equals(right);


                });
            }
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            ValueObject<T> item = obj as ValueObject<T>;

            if ((object)item != null)
                return Equals((T)item);
            else
                return false;

        }

        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            PropertyInfo[] publicProperties = this.GetType().GetProperties();


            if (publicProperties != null && publicProperties.Any())
            {
                foreach (var item in publicProperties)
                {
                    object value = item.GetValue(this, null);

                    if (value != null)
                    {

                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                        hashCode = hashCode ^ (index * 13);//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                }
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);

        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }
    }
}
