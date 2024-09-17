using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly EshopDbContext _context;
        public CartItemRepository(EshopDbContext context)
        {
            _context = context;
        }

        public IQueryable<CartItem> AsQueryable()
        {
            return _context.CartItems.AsQueryable();
        }

        public async Task<ICollection<CartItem>?> GetAllAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems.FindAsync(id);
        }

        public async Task<ICollection<CartItem>?> GetByUserIdAsync(int userId)
        {
            return await _context.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
        }

        public async Task<CartItem?> GetByUserIdAndProductIdAsync(int userId, int productId)
        {
            return await _context.CartItems.Where(ci => ci.UserId == userId && ci.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            return cartItem;
        }

        public async Task<CartItem?> UpdateAsync(CartItem cartItem)
        {
            var updatedCartItem = await _context.CartItems.FindAsync(cartItem.Id);
            if (updatedCartItem == null)
            {
                return null;
            }
            updatedCartItem.UserId = cartItem.UserId;
            updatedCartItem.ProductId = cartItem.ProductId;
            return updatedCartItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var removedCartItem = await _context.CartItems.FindAsync(id);
            if (removedCartItem == null)
            {
                return false;
            }
            _context.CartItems.Remove(removedCartItem);
            return true;
        }
    }
}
