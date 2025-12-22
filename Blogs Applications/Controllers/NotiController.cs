using Application.Dtos.Action;
using Application.IService;
using Application.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotiController : ControllerBase
    {
        private readonly INotiService _notificationService;
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;
        public NotiController(INotiService notificationService, IJsonFieldsSerializer jsonFieldsSerializer)
        {
            _notificationService = notificationService;
            _jsonFieldsSerializer = jsonFieldsSerializer;
        }

        [HttpGet("GetUserNotifications")]
        public async Task<IActionResult> GetAdminNotifications(int adminId)
        {
            var result = await _notificationService.GetAdminNotificationsAsync(adminId);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notifications loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpPost("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllAsRead(int adminId)
        {
            var result = await _notificationService.MarkAllAsReadAsync(adminId);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notification Marked successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpGet("MarkAsReadAsync")]
        public async Task<IActionResult> MarkAsReadAsync(int notificationId)
        {
            var result = await _notificationService.MarkAsReadAsync(notificationId);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notification Marked successfully", StatusCodes.Status200OK, result), string.Empty));
        }










    }
}
