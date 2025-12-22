using Application.Dto.ArticleNotification;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class NotiProfile:Profile
    {
        public NotiProfile()
        {
            CreateMap<CreateNoti, ArticleNotification>();
            CreateMap<ArticleNotification, NotiDto>();
        }
    }
}
