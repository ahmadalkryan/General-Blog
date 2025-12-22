using Applicaion.Dto.ArticleDto;
using Application.Dto.ArticleNotification;
using Application.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface INotiService
    {
        Task<NotiDto> CreateArticleNotification(CreateNoti createNotifcation);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<IEnumerable<NotiDto>> GetAdminNotificationsAsync(int userId);

        Task NotifyNewGlobalMessageAsync(ArticleDto ArticleDto);
    }
}
