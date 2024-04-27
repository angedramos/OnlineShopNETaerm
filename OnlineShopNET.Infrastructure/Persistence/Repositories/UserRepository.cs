using Microsoft.EntityFrameworkCore;
using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Data;
using OnlineShopNET.Infrastructure.Persistence.Interfaces;

namespace OnlineShopNET.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineShopDbContext _dbContext;
        public UserRepository(OnlineShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUsers()
        { 
            return await _dbContext.Users.ToListAsync();
        }
        public async Task<User> GetUserById(int ID)
        {
            var getCredentials = await _dbContext.Users.FirstOrDefaultAsync(x => x.user_id == ID);
            if (getCredentials != null)
                return getCredentials;
            return null;
        }

        public async Task<User> GetUserByUsernameandPassword(string username, string password)
        {
            var getCredentials = await _dbContext.Users.FirstOrDefaultAsync(x => x.username == username && x.password == password);
            if (getCredentials != null)
                return getCredentials;
            return null;
        }

        public async Task<User> CreateUser(User user)
        {
            var checkUserByUsername = await _dbContext.Users.FirstOrDefaultAsync(x => x.username == user.username);
            var checkUserByEmail = await _dbContext.Users.FirstOrDefaultAsync(x => x.email == user.email);

            if (checkUserByUsername != null || checkUserByEmail != null)
                return null;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

    }
}
