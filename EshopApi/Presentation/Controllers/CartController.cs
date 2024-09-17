using Asp.Versioning;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EshopApi.Domain.DTOs;
using EshopApi.Application.Interfaces;

namespace EshopApi.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize(Policy = "RequireUserPermission")]
    public class CartController : ControllerBase
    {
        private readonly IUserService _userservice;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public CartController(IUserService userservice, IProductService productService, ICartService cartService)
        {
            _userservice = userservice;
            _productService = productService;
            _cartService = cartService;
        }

        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var user = await _userservice.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var cartItems = await _cartService.GetCartItemByUserIdAsync(user.Id);
            var products = new List<ProductDTO>();
            if (cartItems != null)
            {
                foreach (var cartItem in cartItems)
                {
                    var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }
            }
            return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
            {
                Status = true,
                Message = "Get all cart items successfully",
                Data = products
            });
        }

        [HttpGet("getCart/{userId}")]
        [Authorize(Policy = "RequireManagerPermission")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cartItems = await _cartService.GetCartItemByUserIdAsync(userId);
            return Ok(new ResponseWrapperDTO<ICollection<CartItemDTO>>()
            {
                Status = true,
                Message = "Get all cart items by user id successfully",
                Data = cartItems
            });
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart(AddToCartReqDTO requestDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var user = await _userservice.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var addedItem = await _cartService.AddNewCartItemAsync(new CartItemNewDTO
            {
                UserId = user.Id,
                ProductId = requestDto.Id
            });
            if (addedItem == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Add item to cart failed",
                });
            }
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Add item to cart successfully",
            });
        }

        [HttpPost("removeFromCart")]
        public async Task<IActionResult> RemoveFromCart(RemoveFromCartReqDTO requestDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return NotFound(new ResponseWrapperDTO<ICollection<string>>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var user = await _userservice.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<ICollection<string>>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var removedItem = await _cartService.GetCartItemByUserIdAndProductIdAsync(user.Id, requestDto.Id);
            if (removedItem == null)
            {
                return NotFound(new ResponseWrapperDTO<ICollection<string>>()
                {
                    Status = false,
                    Message = "Invalid item",
                });
            }
            var result = await _cartService.DeleteCartItemAsync(removedItem.Id);
            if (result == false)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Remove item from cart failed",
                });
            }
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Remove item from cart successfully",
            });
        }
    }
}
