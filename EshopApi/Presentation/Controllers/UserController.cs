using Asp.Versioning;
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
    // [Authorize(Roles = "admin")]
    [Authorize(Policy = "RequireAdminPermission")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new ResponseWrapperDTO<ICollection<UserDTO>>()
            {
                Status = true,
                Message = "Get all users successfully",
                Data = users
            });
        }

        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<UserDTO>()
                {
                    Status = false,
                    Message = "Get user by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<UserDTO>()
            {
                Status = true,
                Message = "Get user by id successfully",
                Data = user
            });
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(UserNewDTO userNewDto)
        {
            var addedUser = await _userService.AddNewUserAsync(userNewDto);
            if (addedUser == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Add new user failed"
                });
            }
            return Ok(new ResponseWrapperDTO<UserDTO>()
            {
                Status = true,
                Message = "Add new user successfully!!!",
                Data = addedUser
            });
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO userDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update user failed"
                });
            }
            return Ok(new ResponseWrapperDTO<UserDTO>()
            {
                Status = true,
                Message = "Update user successfully!!!",
                Data = updatedUser
            });
        }

        [HttpPut("updateUser/{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDto)
        {
            if ((id == 0) || (userDto.Id == 0) || (id != userDto.Id))
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid user id"
                });
            }
            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update user by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<UserDTO>()
            {
                Status = true,
                Message = "Update user by id successfully!!!",
                Data = updatedUser
            });
        }

        [HttpPut("updateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword(UpdatePasswordReqDTO requestDto)
        {
            var updatedUser = await _userService.UpdateUserPasswordAsync(requestDto.UserId, requestDto.Password);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update user password by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<UserDTO>()
            {
                Status = true,
                Message = "Update user password by id successfully!!!",
                Data = updatedUser
            });
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result == false)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Delete user by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Delete user by id successfully!!!",
            });
        }
    }
}
