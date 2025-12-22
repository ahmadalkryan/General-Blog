using Applicaion.Dto.UserDto;
using Applicaion.IRepository;
using Application.Dto.Notification;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IRepository<Notification> _notificationRepository;
        private  readonly IUserService _userService;
        private readonly IRepository<User> _repositoryUser;
        private readonly IMapper _mapper;

        

        public NotificationService(IHubContext<NotificationHub> notificationHub , IUserService userService ,IMapper mapper ,IRepository<Notification> repository, IRepository<User> repository1)
        {
            _notificationHub = notificationHub;
            _repositoryUser = repository1;
            _notificationRepository = repository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task NotifyNewGlobalMessageAsync(GlobalMessage message, UserDto sender)
        {
            var userIds = _repositoryUser.GetAllAsync();
            var allUserIds = (await userIds).Where(u => u.ID != sender.Id).Select(u => u.ID).ToList();  

            var notifications = allUserIds.Select(userId => new Notification
            {
                Title = " New Message ✨",
                MessageNotification = $"{sender.UserName}: send message",
                SentAt = DateTime.UtcNow,
                IsRead = false,
                receiverId = userId,
                MessageId = message.ID
            }).ToList();

            await _notificationRepository.InsertRangeAsync(notifications);

            foreach (var notification in notifications)
            {
                var notificationDto = _mapper.Map<NotificationDto>(notification);
               

                await _notificationHub.Clients.User(notification.receiverId.ToString())
                    .SendAsync("ReceiveNotification", notificationDto);
            }


        }

      public async  Task<NotificationDto> CreateNotification(CreateNotifcation createNotifcation)
        {
            var res= await _notificationRepository.Insertasync(_mapper.Map<Notification>(createNotifcation));
            var result =  _mapper.Map<NotificationDto>(res);
            await _notificationHub.Clients.User(createNotifcation.receiverId.ToString())
               .SendAsync("ReceiveNotification", result);

           return result;
        }

        public async Task<NotificationDto> GetNotificationById(int id)
        {
            var res = await _notificationRepository.GetById(id);
            return _mapper.Map<NotificationDto>(res);
        }


         public async  Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var filteredNotifications = notifications
                .Where(x => x.receiverId == userId && !x.IsRead)
                .OrderByDescending(x => x.SentAt)
                .ToList();
            var result = _mapper.Map<IEnumerable<NotificationDto>>(filteredNotifications);
            return result;
           

        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            
            var notifications = await _notificationRepository.GetAllAsync();
            var userNotifications = notifications
                .Where(n => n.receiverId == userId && !n.IsRead)
                .ToList();

            if (!userNotifications.Any())
                return true;

            foreach (var notification in userNotifications)
            {
                notification.IsRead = true;
            }
            await _notificationRepository.UpdateRangeAsync(userNotifications);

            

            return true;
        }

      public  async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var not = await _notificationRepository.GetById(notificationId);

            not.IsRead = true;
            await _notificationRepository.UpdateAsync(not);

            return true;
        }
        private string TruncateMessage(string message, int maxLength)
        {
            if (string.IsNullOrEmpty(message)) return message;

            return message.Length <= maxLength
                ? message
                : message.Substring(0, maxLength) + "...";
        }

      public async  Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            var res = await _notificationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NotificationDto>>(res);
        }
    }
}
