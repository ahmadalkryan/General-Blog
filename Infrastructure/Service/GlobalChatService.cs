using Applicaion.IRepository;
using Application.Dto.Message;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class GlobalChatService : IGlobalChatService
    {
        private readonly IUserService _userService;
        private readonly IRepository<GlobalMessage> _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public GlobalChatService(IUserService userService ,IRepository<GlobalMessage> repository
            , IMapper mapper,INotificationService notificationService )
        {
            
            _userService = userService;
            _mapper = mapper;
            _repository = repository;
            _notificationService = notificationService;

        }
        public async Task<MessgeDto> GetMessageAsync(int messageId)
        {
            var result = await _repository.GetById(messageId);

            return  _mapper.Map<MessgeDto>(result);
        }

        public async Task<IEnumerable<MessgeDto>> GetRecentMessage()
        {
           var result = await _repository.GetAllAsync();

            var recent = result.Where(x => !x.IsDeleted).OrderByDescending(x => x.SentAt).ToList();

            return _mapper.Map<IEnumerable<MessgeDto>>(recent);
        }

        public async Task<MessgeDto> SendMessage(CrMessageDto crMessage)
        {
            var user = await _userService.GetCurrentUserAsync();
            var create = new CreateMessageDto
            {
                senderId = user.Id,
                Content = crMessage.Content,
                MessageType = crMessage.MessageType,

            };
            var result = _mapper.Map<GlobalMessage>(create);
         
            var res = await _repository.Insertasync(result);
            await _notificationService.NotifyNewGlobalMessageAsync(result, user);
            return _mapper.Map<MessgeDto>(res);
        }

       public async Task<MessgeDto> DeleteMessageAsync(int messageId)
        {
            var a = await _repository.GetById(messageId);

            var res = await _repository.RemoveAsync(a);

            return _mapper.Map<MessgeDto>(res);
        }













    }
}
