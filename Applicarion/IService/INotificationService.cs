using Applicaion.Dto.UserDto;
using Application.Dto.Notification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface INotificationService
    {
        Task <NotificationDto> CreateNotification(CreateNotifcation createNotifcation);
        Task<NotificationDto> GetNotificationById(int id);
        Task NotifyNewGlobalMessageAsync(GlobalMessage message, UserDto sender);

        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);

    }
}
