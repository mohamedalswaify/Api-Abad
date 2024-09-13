namespace todoApiAbadnet.DTO
{
    public class TabbyWebhookPayload
    {
        public string Event { get; set; }
        public WebhookData Data { get; set; }
    }

    public class WebhookData
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
    }
}
