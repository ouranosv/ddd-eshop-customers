namespace Common.Dtos
{
    public class Result<T>
    {
        public T Value { get; private set; }
        public string Error { get; private set; }
        public bool IsFailure => Error != null;

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Fail(string error)
        {
            return new Result<T>(default, error);
        }

        private Result(T value, string error = null)
        {
            Value = value;
            Error = error;
        }
    }
}
