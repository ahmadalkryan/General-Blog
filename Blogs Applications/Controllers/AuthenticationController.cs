using Applicarion.Dto.UserDto;
using Applicarion.IService;
using Application.Dtos.Action;
using Application.Serializer;
using Azure;
using Infrastructure.Service.JwtService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;


        public AuthenticationController(IUserService userService ,IJsonFieldsSerializer jsonFieldsSerializer ,IJwtService jwtService)
        {
            _userService = userService;
            _jsonFieldsSerializer = jsonFieldsSerializer;
            _jwtService = jwtService;
                
        }

        [HttpPost]

        public async Task<IActionResult > Register([FromBody]RegisterDto registerDto)
        {

            var result = await _userService.RegisterAsync(registerDto);

           return new  RawJsonActionResult(_jsonFieldsSerializer.Serialize(
               new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));


        }


        [HttpPost]

        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {


            var userdto = await _userService.LoginAsync(loginDto);

            var _user = await _userService.GetUser(userdto.Id);

            var token = _jwtService.GenerateToken(_user);

            var response = new LoginResponse
            {
                Token = token,
                User = userdto,
                Expires = DateTime.Now.AddHours(3)
            };

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, response), string.Empty));




        }

        [HttpGet]

        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _userService.GetCurrentUserAsync();

             return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }

        [HttpPut]

        public async Task<IActionResult> UpdateRole(UpdateUserRoleDto updateUserRoleDto)
        {
            var result = await _userService.UpdateUserRoleAsync( updateUserRoleDto);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]

        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUser();
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpGet]

        public async Task<IActionResult> GetByIDUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }





        // client side logout remove token from STORAGE
        [HttpPost]

        public async Task<IActionResult > Logout()
        {
            var result = await _userService.Logout();
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
            new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }






    }
}
