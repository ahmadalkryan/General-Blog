using Applicarion.Dto.UserDto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Mapper
{
    public class UserProfile:Profile
    {

        public UserProfile()
        {
            CreateMap<User, UserDto>().ForMember(dest=>dest.Id , opt =>opt.MapFrom(src =>src.ID));
           // CreateMap<RegisterDto,User >().
        }
    }
}
