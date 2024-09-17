using Asp.Versioning;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using EshopApi.Domain.DTOs;
using EshopApi.Application.Interfaces;

namespace EshopApi.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDTO requestDto)
        {
            var user = await _userService.VerifyUserPasswordAsync(requestDto.Username, requestDto.Password);
            if (user == null)
            {
                return Unauthorized(new ResponseWrapperDTO<string>
                {
                    Status = false,
                    Message = "Invalid username or password"
                });
            }
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Role, user.Role),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true // IsPersistent = requestDto.IsRememberMe
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
            return Ok(new ResponseWrapperDTO<LoginRespDTO>
            {
                Status = true,
                Message = "Logged in successfully",
                Data = new LoginRespDTO
                {
                    Username = user.Username,
                    Role = user.Role
                }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if ((HttpContext.User.Identity == null) || !HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(new ResponseWrapperDTO<string>
                {
                    Status = true,
                    Message = "User was not logged in or already logged out"
                });
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Logged out successfully"
            });
        }
    }
}
