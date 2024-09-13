namespace todoApiAbadnet.DTO
{
    public class OrderDetails
    {
        public string order_id { get; set; }
        public TotalAmount total_amount { get; set; }
        public List<Items> items { get; set; }
        public TotalAmount discount_amount { get; set; }
        public TotalAmount shipping_amount { get; set; }
        public TotalAmount tax_amount { get; set; }
        public ShippingInfo shipping_info { get; set; }
    }

    public class TotalAmount
    {
        public int amount { get; set; }
        public string currency { get; set; }

    }

    public class Items
    {
        public string name { get; set; }
        public string type { get; set; }
        public string reference_id { get; set; }
        public string sku { get; set; }
        public int quantity { get; set; }
        public TotalAmount discount_amount { get; set; }
        public TotalAmount tax_amount { get; set; }
        public TotalAmount unit_price { get; set; }
        public TotalAmount total_amount { get; set; }

    }
    public class ShippingInfo
    {
        public DateTime shipped_at { get; set; }
        public string shipping_company { get; set; }
        public int tracking_number { get; set; }
        public string tracking_url { get; set; }
    }
}
