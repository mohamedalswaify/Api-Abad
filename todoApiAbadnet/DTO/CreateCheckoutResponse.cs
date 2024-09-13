using Newtonsoft.Json;

namespace todoApiAbadnet.DTO
{
    public class CreateCheckoutResponse
    {
        public int Id { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("checkout_id")]
        public string CheckoutId { get; set; }

        [JsonProperty("checkout_url")]
        public string CheckoutUrl { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

