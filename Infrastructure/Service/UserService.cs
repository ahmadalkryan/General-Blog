using Applicarion.Dto.UserDto;
using Applicarion.IRepository;
using Applicarion.IService;
using AutoMapper;
using Azure.Core;
using Domain.Entities;
using Infrastructure.Service.HashPassword;
using Infrastructure.Service.JwtService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHash _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private IJwtService _jwtService;
        private readonly ITokenBlackList _tokenBlackList;

        public UserService(IRepository<User> repository,ITokenBlackList tokenBlackList, IPasswordHash passwordHasher,IMapper mapper ,IHttpContextAccessor httpContextAccessor ,IJwtService jwtService)
        {
            _repository=repository;
            _passwordHasher=passwordHasher;
             _contextAccessor=httpContextAccessor;
            _jwtService=jwtService;
            _mapper =mapper;
            _tokenBlackList=tokenBlackList;

        }


        public async  Task<IEnumerable<UserDto>> GetAllUser()
        {
            return _mapper.Map<IEnumerable<UserDto>>( await _repository.GetAllAsync() );
        }

       

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetById(id);

            return _mapper.Map<UserDto>( user );
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            
           var check =  GetAllUser().Result.FirstOrDefault(x=>x.Email==registerDto.Email); 
            if (check != null)
            {
                throw new Exception("User already registerd");
            }
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
                Role = "User"       //default
            };

            await _repository.Insertasync(user);

            return _mapper.Map<UserDto>(user);

        }

        public async Task<UserDto> UpdateUserRoleAsync( UpdateUserRoleDto updateUserRoleDto)
        {
            User user =  await _repository.GetById(updateUserRoleDto.Id);
            user.Role = updateUserRoleDto.Role;
            await _repository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user) ;

        }

        public  async Task<UserDto> GetCurrentUserAsync()
        {
            var userID = GetCurrentUserId();

            var user = await GetUserByIdAsync(userID);
            return _mapper.Map<UserDto>(user);

            
        }
        private int GetCurrentUserId()
        {
            var userCliamId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             if(int.TryParse(userCliamId,out int userId)) { 
                return userId;
            }
             throw new UnauthorizedAccessException("User not authenticated");
        }


        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            
            var user =  _repository.GetAllAsync().Result
                .FirstOrDefault(x=>x.Email==loginDto.Email);

            bool isvalid = user != null && _passwordHasher.VerifyPassword(loginDto.Password ,user.PasswordHash);
            if(! isvalid)
            {
                throw new Exception("Credintail wrong");
            }

            return _mapper.Map<UserDto>(user);
            
        }

          public async Task<User> GetUser(int userID)
        {
            var res = await _repository.GetById(userID);
            return res;
        }

      public async   Task<LogoutDto> Logout()
        {
            var token = _contextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            await _tokenBlackList.BlacklistAsync(token,DateTime.Now.AddHours(2));
            var result = new LogoutDto()
            {
                message ="success",
                LogoutTime = DateTime.Now,
                userId = GetCurrentUserId(),
            };

            return result;
        }
    }
}
