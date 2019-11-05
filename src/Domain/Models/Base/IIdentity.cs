namespace Domain.Models.Base
{
    public interface IIdentity<T>
    {
        T Value { get; }
    }
}
