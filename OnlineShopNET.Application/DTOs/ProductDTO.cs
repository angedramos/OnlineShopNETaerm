namespace OnlineShopNET.Application.DTOs
{
    public class ProductDTO
    {
        public string product_name { get; set; }
        public string product_description { get; set; }
        public int product_quantity { get; set; }
        public decimal product_price { get; set; }
        public bool product_status { get; set; }
        public int categoryId { get; set; }
    }
}
