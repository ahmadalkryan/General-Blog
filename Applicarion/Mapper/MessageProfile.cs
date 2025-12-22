using Application.Dto.Message;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MessageProfile:Profile
    {
        public MessageProfile()
        {
            CreateMap<GlobalMessage, MessgeDto>().ForMember(dest=>dest.UserName ,opt=>opt.MapFrom(src=>src._user.UserName));

            CreateMap<CreateMessageDto, GlobalMessage>().ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).
                ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => DateTime.UtcNow));     
        }
    }
}
