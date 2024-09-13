using Newtonsoft.Json;

namespace todoApiAbadnet.DTO
{
    public class AuthorizedAmount
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
