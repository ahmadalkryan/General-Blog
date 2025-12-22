using Applicaion.Dto.ArticleDto;
using Application.Dto.Message;
using Application.Dtos.Action;
using Application.IService;
using Application.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;
        private readonly IGlobalChatService _chatService;

        public ChatController(IGlobalChatService globalChatService ,IJsonFieldsSerializer jsonFieldsSerializer)
        {
                     _chatService = globalChatService;   
                    _jsonFieldsSerializer = jsonFieldsSerializer;
            
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MessgeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMessage(CrMessageDto crMessageDto)
        {
            var result =await _chatService.SendMessage(crMessageDto);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
               new ApiResponse(true, "Message sent successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MessgeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentMessage()
        {

            var result = await _chatService.GetRecentMessage();

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
               new ApiResponse(true, "Message fetch successfully", StatusCodes.Status200OK, result), string.Empty));

        }


        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<MessgeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = await _chatService.DeleteMessageAsync(messageId);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
               new ApiResponse(true, "Message deleted successfully", StatusCodes.Status200OK, result), string.Empty));
        }




    }
}
