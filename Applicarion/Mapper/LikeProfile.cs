using Application.Dto.Like;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class LikeProfile:AutoMapper.Profile
    {
        public LikeProfile()
        {
            CreateMap<Like, LikeDto>().ForMember(des =>des.Username,
                opt => opt.MapFrom(src => src._user.UserName));

            CreateMap<CreateLike, Like>().ForMember(des=>des.CreatedAt,opt=> opt.MapFrom(src => DateTime.UtcNow));


        }
    }
}
