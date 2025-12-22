using Application.Dto.Profile;
using Application.Dtos.Action;
using Application.IService;
using Application.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
          private readonly IJsonFieldsSerializer _jsonFieldsSerializer ;
        private readonly IProfileService _profileService;


        public ProfileController(IProfileService  profileService ,IJsonFieldsSerializer jsonFieldsSerializer)
        {
            _profileService = profileService;
            _jsonFieldsSerializer = jsonFieldsSerializer;

        }
        [HttpGet]

        public async Task<IActionResult> GetProfile(int userId)
        {

            var result = await _profileService.GetProfileByUserID(userId);


            
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Profiel loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpPost]

        public async Task<IActionResult> CreateProfile([FromBody]CreateProfile profile)
        {
            var result = await _profileService.CreateProfile(profile);

              return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Profiel loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteProfiel(int profileId)
        {
            var result = await _profileService.DeleteProfile(profileId);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(new ApiResponse(true, "Profiel loaded successfully", StatusCodes.Status200OK, result),
                string.Empty));
        }

        [HttpGet]

        public async Task<IActionResult> GetALl()
        {
            var result = await _profileService.GetAllProfiles();

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(new ApiResponse(true, "Profiel loaded successfully", StatusCodes.Status200OK, result),
                string.Empty));
        }
    }
}
