using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private UserDTO ToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<UserDTO>?> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users?.Select(ToUserDTO).ToList();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return (user != null) ? ToUserDTO(user) : null;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            return (user != null) ? ToUserDTO(user) : null;
        }

        public async Task<UserDTO?> AddNewUserAsync(UserNewDTO userNewDto)
        {
            var addedUser = new User
            {
                Username = userNewDto.Username,
                Role = userNewDto.Role,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(userNewDto.Password)
            };
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    addedUser = await _unitOfWork.UserRepository.AddAsync(addedUser);
                }, cts.Token);
                return ToUserDTO(addedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDTO?> UpdateUserAsync(UserDTO userDto)
        {
            var updatedUser = await _unitOfWork.UserRepository.GetByIdAsync(userDto.Id);
            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.Username = userDto.Username;
            updatedUser.Role = userDto.Role;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    updatedUser = await _unitOfWork.UserRepository.UpdateAsync(updatedUser);
                }, cts.Token);
                return ToUserDTO(updatedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDTO?> UpdateUserPasswordAsync(int id, string password)
        {
            var updatedUser = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    updatedUser = await _unitOfWork.UserRepository.UpdateAsync(updatedUser);
                }, cts.Token);
                return ToUserDTO(updatedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.UserRepository.DeleteAsync(id);
                }, cts.Token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return false;
            }
        }

        public async Task<UserDTO?> VerifyUserPasswordAsync(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            if ((user == null) || !BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
            {
                return null;
            }
            return ToUserDTO(user);
        }
    }
}
