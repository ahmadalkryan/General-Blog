using Applicaion.Dto.ArticleDto;
using Applicaion.IRepository;
using Application.Dto.ArticleNotification;
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
    public class NotiService : INotiService
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserService _userService;
        private readonly IRepository<ArticleNotification> _repository ;
        public NotiService(IMapper mapper ,IRepository<ArticleNotification> repository ,IUserService userService ,IHubContext<NotificationHub> hubContext)
        { 
            _mapper = mapper;
            _userService = userService;
            _hubContext = hubContext;
            _repository = repository;
            
        }
        public async Task<NotiDto> CreateArticleNotification(CreateNoti createNotifcation)
        {
            var res = _mapper.Map<ArticleNotification>(createNotifcation);
            await _repository.Insertasync(res);

            return _mapper.Map<NotiDto>(res);

        }

        public async Task<IEnumerable<NotiDto>> GetAdminNotificationsAsync(int userId)
        {
            var res =await _repository.GetAllAsync();

            var result = res.Where(x=>x.userId == userId && !x.IsRead).ToList();

            return _mapper.Map<IEnumerable<NotiDto>>(result);

        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var res =(await _repository.GetAllAsync()).Where(x=>x.userId==userId && !x.IsRead).ToList();
            foreach (var item in res)
            {
               item.IsRead = true;
                await _repository.UpdateAsync(item);
            }
            return true;
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var res = await _repository.GetById(notificationId);
            res.IsRead = true;
            await _repository.UpdateAsync(res);
            return res.IsRead;
        }

         public   async Task NotifyNewGlobalMessageAsync(ArticleDto ArticleDto)
        {
            var admins = (await _userService.GetAllUser()).Where(x => x.Role == "Admin").Select(x=>x.Id).ToList();

            var nots = admins.Select(x => new ArticleNotification
            {
                userId = x,
                articleId=ArticleDto.ID,
                Message = $"New article published: {ArticleDto.Title}",
                IsRead = false


            })
            .ToList();

            await _repository.InsertRangeAsync(nots);
            foreach(var not in nots)
            {
                var notiDto = _mapper.Map<NotiDto>(not);
                await _hubContext.Clients.User(not.userId.ToString())
                    .SendAsync("Article Create Waiting For Processing ", notiDto);
            }




        }
    }
}
