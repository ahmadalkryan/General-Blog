using Application.Dto.Message;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IGlobalChatService
    {
        Task<MessgeDto> SendMessage(CrMessageDto crMessage);

        Task<IEnumerable<MessgeDto>> GetRecentMessage();

        Task<MessgeDto> GetMessageAsync(int messageId);

         Task<MessgeDto> DeleteMessageAsync(int messageId);

        //  Task<ChatStatistics> GetChatStatisticsAsync();
    }
}
