using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;

        private CartItemDTO ToCartItemDTO(CartItem cartItem)
        {
            return new CartItemDTO
            {
                Id = cartItem.Id,
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId
            };
        }

        public CartService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<ICollection<CartItemDTO>?> GetAllCartItemsAsync()
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            return cartItems?.Select(ToCartItemDTO).ToList();
        }

        public async Task<CartItemDTO?> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);
            return (cartItem != null) ? ToCartItemDTO(cartItem) : null;
        }

        public async Task<ICollection<CartItemDTO>?> GetCartItemByUserIdAsync(int userId)
        {
            var cartItems = await _cartItemRepository.GetByUserIdAsync(userId);
            return cartItems?.Select(ToCartItemDTO).ToList();
        }

        public async Task<CartItemDTO?> GetCartItemByUserIdAndProductIdAsync(int userId, int productId)
        {
            var cartItem = await _cartItemRepository.GetByUserIdAndProductIdAsync(userId, productId);
            return (cartItem != null) ? ToCartItemDTO(cartItem) : null;
        }

        public async Task<CartItemDTO?> AddNewCartItemAsync(CartItemNewDTO cartItemNewDto)
        {
            var addedCartItem = new CartItem()
            {
                UserId = cartItemNewDto.UserId,
                ProductId = cartItemNewDto.ProductId
            };
            addedCartItem = await _cartItemRepository.AddAsync(addedCartItem);
            return (addedCartItem != null) ? ToCartItemDTO(addedCartItem) : null;
        }

        public async Task<CartItemDTO?> UpdateCartItemAsync(CartItemDTO cartItemDto)
        {
            var updatedCartItem = await _cartItemRepository.GetByIdAsync(cartItemDto.Id);
            if (updatedCartItem == null)
            {
                return null;
            }
            updatedCartItem.UserId = cartItemDto.UserId;
            updatedCartItem.ProductId = cartItemDto.ProductId;
            updatedCartItem = await _cartItemRepository.UpdateAsync(updatedCartItem);
            return (updatedCartItem != null) ? ToCartItemDTO(updatedCartItem) : null;
        }

        public async Task<bool> DeleteCartItemAsync(int id)
        {
            return await _cartItemRepository.DeleteAsync(id);
        }
    }
}
