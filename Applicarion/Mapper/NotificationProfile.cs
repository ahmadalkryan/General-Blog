using Application.Dto.Notification;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class NotificationProfile: Profile
    {
        public NotificationProfile()
        {
            CreateMap<CreateNotifcation, Notification>();
            CreateMap<Notification, NotificationDto>();
        }
    }
}
