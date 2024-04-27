namespace OnlineShopNET.Application.DTOs
{
    public enum UserRoleType
    {
        User = 1,
        Admin = 2
    }
    public class UserDTO 
    {

        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int role_type { get; set; }
        public DateTime created_at { get; set; }

    }


}
