using Newtonsoft.Json;

namespace todoApiAbadnet.Models
{
    public class AuthorizeOrderModel
    {
      
        public int Id { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("order_expiry_time")]
        public DateTime OrderExpiryTime { get; set; }

        [JsonProperty("payment_type")]
        public string? PaymentType { get; set; }

        [JsonProperty("auto_captured")]
        public bool? AutoCaptured { get; set; }


        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }


        public string? paymentStatus { get; set; }


        [JsonProperty("capture_id")]
        public string? CaptureId { get; set; }
    }
}
