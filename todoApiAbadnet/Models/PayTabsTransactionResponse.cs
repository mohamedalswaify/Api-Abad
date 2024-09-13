using System.ComponentModel.DataAnnotations.Schema;

namespace todoApiAbadnet.Models
{
    [NotMapped]
    public class PayTabsTransactionResponse
    {
        public string tran_ref { get; set; }
        public string tran_type { get; set; }
        public string cart_id { get; set; }
        public string cart_description { get; set; }
        public string cart_currency { get; set; }
        public string cart_amount { get; set; }
        public string tran_total { get; set; }
        public string @return { get; set; }
        public string redirect_url { get; set; }
        public int serviceId { get; set; }
        public int profileId { get; set; }
        public int merchantId { get; set; }
        public string trace { get; set; }

        public bool IsSuccess
        {
            get { return !string.IsNullOrWhiteSpace(tran_ref) && !string.IsNullOrWhiteSpace(redirect_url); }
        }
    }

}
