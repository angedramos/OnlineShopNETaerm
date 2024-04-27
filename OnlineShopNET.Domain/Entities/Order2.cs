using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopNET.Domain.Entities
{
    public class Order2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderID { get; set; }

        public int userID { get; set; }

        public string? purchase { get; set; }

        public decimal? totalPrice { get; set; }

        public DateTime? orderDate { get; set; }

    }
}
