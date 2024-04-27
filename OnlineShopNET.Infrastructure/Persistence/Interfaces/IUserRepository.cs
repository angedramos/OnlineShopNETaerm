using OnlineShopNET.Domain.Entities;

namespace OnlineShopNET.Infrastructure.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<User> CreateUser(User user);
        Task<User> GetUserByUsernameandPassword(string username, string password);
        Task<User> GetUserById(int ID);
    }
}
