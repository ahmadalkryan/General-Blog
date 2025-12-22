using Application.Dto.Like;
using Application.Dtos.Action;
using Application.IService;
using Application.Serializer;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;
        public NotificationController(INotificationService notificationService ,IJsonFieldsSerializer jsonFieldsSerializer)
        {
            _notificationService = notificationService;
            _jsonFieldsSerializer = jsonFieldsSerializer;
        }
        [HttpGet("GetUserNotifications")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var result = await _notificationService.GetUserNotificationsAsync(userId);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notifications loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpGet("GetAllNotifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var result = await _notificationService.GetAllNotificationsAsync();

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notifications loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpGet("GetUnreadUserNotificationsCount")]
        public async Task<IActionResult> GetUnreadUserNotificationsCount(int userId)
        {
            var res = await _notificationService.GetUserNotificationsAsync(userId);
            int result = res.Count();

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notifications loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet("GetNotificationById")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            var result = await _notificationService.GetNotificationById(notificationId);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notification loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpPost ("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notification Marked successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpGet("MarkAsReadAsync")]
        public async Task<IActionResult> MarkAsReadAsync(int notificationId )
        {
            var result = await _notificationService.MarkAsReadAsync(notificationId);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "notification Marked successfully", StatusCodes.Status200OK, result), string.Empty));
        }

    }
}
