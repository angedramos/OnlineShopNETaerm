using System.ComponentModel.DataAnnotations;

namespace OnlineShopNET.Application.DTOs
{
    public class LoginUserDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string username {  get; set; }
        [Required(AllowEmptyStrings = false)]
        public string password { get; set; }
    }
}
