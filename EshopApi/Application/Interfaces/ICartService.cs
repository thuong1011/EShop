using EshopApi.Domain.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface ICartService
    {
        Task<ICollection<CartItemDTO>?> GetAllCartItemsAsync();
        Task<CartItemDTO?> GetCartItemByIdAsync(int id);
        Task<ICollection<CartItemDTO>?> GetCartItemByUserIdAsync(int userId);
        Task<CartItemDTO?> GetCartItemByUserIdAndProductIdAsync(int userId, int productId);
        Task<CartItemDTO?> AddNewCartItemAsync(CartItemNewDTO cartItemNewDto);
        Task<CartItemDTO?> UpdateCartItemAsync(CartItemDTO cartItemDto);
        Task<bool> DeleteCartItemAsync(int id);
    }
}
