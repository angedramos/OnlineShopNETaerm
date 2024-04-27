using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopNET.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productId { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public int product_quantity { get; set; }
        public decimal product_price { get; set; }
        public bool product_status { get; set; }
        public int categoryId { get; set; }
    }
}
