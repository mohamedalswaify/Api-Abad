using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApiAbadnet.DTO
{
    public class AuthorizeOrderResponse
    {

        public int Id { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("order_expiry_time")]
        public DateTime OrderExpiryTime { get; set; }

        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }

        [JsonProperty("auto_captured")]
        public bool AutoCaptured { get; set; }

        [NotMapped]
        [JsonProperty("authorized_amount")]
        public AuthorizedAmount AuthorizedAmount { get; set; }

        [JsonProperty("capture_id")]
        public string CaptureId { get; set; }
    }
}
