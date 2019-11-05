namespace Common.Dtos
{
    public class ErrorMessage
    {
        public string Message { get; }

        public ErrorMessage(string message)
        {
            Message = message;
        }
    }
}
