using EshopApi.Domain.Entities;

namespace EshopApi.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        IQueryable<CartItem> AsQueryable();
        Task<ICollection<CartItem>?> GetAllAsync();
        Task<CartItem?> GetByIdAsync(int id);
        Task<ICollection<CartItem>?> GetByUserIdAsync(int userId);
        Task<CartItem?> GetByUserIdAndProductIdAsync(int userId, int productId);
        Task<CartItem> AddAsync(CartItem cartItem);
        Task<CartItem?> UpdateAsync(CartItem cartItem);
        Task<bool> DeleteAsync(int id);
    }
}
