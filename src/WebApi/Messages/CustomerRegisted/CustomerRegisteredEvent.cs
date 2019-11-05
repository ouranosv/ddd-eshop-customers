namespace WebApi.Messages.CustomerRegistered
{
    public class CustomerRegisteredEvent
    {
        public string CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
