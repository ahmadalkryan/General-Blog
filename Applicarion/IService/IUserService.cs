using Applicarion.Dto.UserDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> UpdateUserRoleAsync(UpdateUserRoleDto updateUserRoleDto);
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> GetCurrentUserAsync();
        Task<User> GetUser(int userID);
        Task<UserDto> GetUserByIdAsync(int id);

        Task<LogoutDto> Logout();
        Task<IEnumerable<UserDto>> GetAllUser();
    }
}
