

namespace todoApiAbadnet.DTO
{
    public class AvailableProducts
    {
        public List<Installment> installments { get; set; }
    }

    public class Configuration
    {
        public string currency { get; set; }
        public string app_type { get; set; }
        public bool new_customer { get; set; }
        public object available_limit { get; set; }
        public object min_limit { get; set; }
        public AvailableProducts available_products { get; set; }
        public ExtraAvailableProducts extra_available_products { get; set; }
        public string country { get; set; }
        public DateTime expires_at { get; set; }
        public bool is_bank_card_required { get; set; }
        public object blocked_until { get; set; }
        public bool hide_closing_icon { get; set; }
        public object pos_provider { get; set; }
        public bool is_tokenized { get; set; }
        public string disclaimer { get; set; }
        public string help { get; set; }
        public bool is_ipqs_required { get; set; }
        public object orders_count { get; set; }
        public object has_identity { get; set; }
        public MonthlyBilling monthly_billing { get; set; }
        public Products products { get; set; }
        public string order_details_mode { get; set; }
    }

    public class Customer
    {
        public object id { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public object is_identity_auth_skipped { get; set; }
    }

    public class ExtraAvailableProducts
    {
    }

    public class Installment
    {
        public string downpayment { get; set; }
        public string downpayment_percent { get; set; }
        public object downpayment_increased_reason { get; set; }
        public string amount_to_pay { get; set; }
        public object old_downpayment_total { get; set; }
        public string downpayment_total { get; set; }
        public string total_service_fee { get; set; }
        public string service_fee_policy { get; set; }
        public string order_amount { get; set; }
        public DateTime next_payment_date { get; set; }
        public List<Installments> installments { get; set; }
        public bool pay_after_delivery { get; set; }
        public string pay_per_installment { get; set; }
        public string web_url { get; set; }
        public string qr_code { get; set; }
        public object original_type { get; set; }
        public string status { get; set; }
        public int id { get; set; }
        public int installments_count { get; set; }
        public string installment_period { get; set; }
        public string service_fee { get; set; }
        public string due_date { get; set; }
        public object old_amount { get; set; }
        public string amount { get; set; }
        public string principal { get; set; }
        public string type { get; set; }
        public bool is_available { get; set; }
        public object rejection_reason { get; set; }
    }

    public class Installments
    {
        public string due_date { get; set; }
        public object old_amount { get; set; }
        public string amount { get; set; }
        public string principal { get; set; }
        public string service_fee { get; set; }
    }

    public class InstallmentPlan
    {
        public Installments installments { get; set; }
    }

    public class Juicyscore
    {
        public string session_id { get; set; }
        public string referrer { get; set; }
        public string time_zone { get; set; }
        public string useragent { get; set; }
    }

    public class Merchant
    {
        public string name { get; set; }
        public string address { get; set; }
        public string logo { get; set; }
    }

    public class MonthlyBilling
    {
        public int due_day { get; set; }
    }


    public class CheckOutPayment
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime expires_at { get; set; }
        public string status { get; set; }
        public bool is_test { get; set; }
        public Product product { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public Buyer buyer { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public Order order { get; set; }
        public List<object> captures { get; set; }
        public List<object> refunds { get; set; }
        public BuyerHistory buyer_history { get; set; }
        public List<OrderHistory> order_history { get; set; }
        public Meta meta { get; set; }
        public bool cancelable { get; set; }
        public Attachment attachment { get; set; }
    }

    public class Product
    {
        public string type { get; set; }
        public int installments_count { get; set; }
        public string installment_period { get; set; }
    }

    public class Products
    {
        public Installments installments { get; set; }
    }

    public class Checkout
    {
        public string id { get; set; }
        public object warnings { get; set; }
        public Configuration configuration { get; set; }
        public string api_url { get; set; }
        public object token { get; set; }
        public string flow { get; set; }
        public CheckOutPayment payment { get; set; }
        public string status { get; set; }
        public Customer customer { get; set; }
        public Juicyscore juicyscore { get; set; }
        public MerchantUrls merchant_urls { get; set; }
        public object product_type { get; set; }
        public string lang { get; set; }
        public string locale { get; set; }
        public object seon_session_id { get; set; }
        public Merchant merchant { get; set; }
        public string merchant_code { get; set; }
        public bool terms_accepted { get; set; }
        public object promo { get; set; }
        public InstallmentPlan installment_plan { get; set; }
        public bool is_ipqs_requested { get; set; }
    }
}
