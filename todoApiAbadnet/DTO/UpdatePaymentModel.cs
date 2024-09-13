namespace todoApiAbadnet.DTO
{
    public class OrderUpdate
    {
        public string reference_id { get; set; }
    }

    public class UpdatePaymentModel
    {
        public OrderUpdate order { get; set; }
    }
}
