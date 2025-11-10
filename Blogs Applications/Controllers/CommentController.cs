using Applicarion.Dto.CommentDto;
using Applicarion.IService;
using Application.Dtos.Action;
using Application.Serializer;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;

        public CommentController(IJsonFieldsSerializer jsonFieldsSerializer ,ICommentService commentService)
        {
            _jsonFieldsSerializer = jsonFieldsSerializer;
            _commentService = commentService;
            
        }


        
        [HttpGet]
        public async Task<IActionResult>  GetAllComment()
        {
           var result  = await _commentService.GetAllComments();
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }

       
        [HttpGet]
        public async Task<IActionResult> GetByIdComment( int id)
        {
            var result = await _commentService.GetCommentByID(id);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentDto createCommentDto)
        {
            var result = await _commentService.CreateComment(createCommentDto);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));


        }

      
        [HttpPut]
        public async Task<IActionResult> UpdateComment( [FromBody] UpdateCommentDto updateCommentDto)
        {
            var result = await _commentService.UpdateComment(updateCommentDto);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
            new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }

       
        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromQuery] int id)
        {
            var result = await _commentService.DeleteComment(id);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
            new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }
    }
}
