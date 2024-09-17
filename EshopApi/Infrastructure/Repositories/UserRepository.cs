using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EshopDbContext _context;
        public UserRepository(EshopDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> AsQueryable()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<ICollection<User>?> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var updatedUser = await _context.Users.FindAsync(user.Id);
            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.Username = user.Username;
            updatedUser.Role = user.Role;
            updatedUser.HashedPassword = user.HashedPassword;
            return updatedUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var removedUser = await _context.Users.FindAsync(id);
            if (removedUser == null)
            {
                return false;
            }
            _context.Users.Remove(removedUser);
            return true;
        }
    }
}
