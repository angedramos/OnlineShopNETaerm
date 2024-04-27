namespace OnlineShopNET.Application.DTOs
{
    public class CreateOrderDTO
    {
        public int userID { get; set; }

        public string? purchase { get; set; }

        public decimal? OrderTotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }
    }
}
