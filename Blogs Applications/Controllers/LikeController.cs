using Application.Dto.Like;
using Application.Dtos.Action;
using Application.IService;
using Application.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LikeController : ControllerBase
    {

        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService ,IJsonFieldsSerializer jsonFieldsSerializer)
        {
            _jsonFieldsSerializer  = jsonFieldsSerializer;
            _likeService = likeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLikee(int articleId)
        {
            var result = await _likeService.GetAllLikesForArticle(articleId);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "ArticleLike loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpPost]
        public async Task<IActionResult> CreateLike([FromBody] crLike like)
        {
            var result = await _likeService.CreateLike(like);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Like created successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLike([FromQuery] int id)
        {
            var result = await _likeService.DeleteLike(id);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
               new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }
    }
}
