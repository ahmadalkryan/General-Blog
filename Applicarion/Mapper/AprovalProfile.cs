using Application.Dto.Approval;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class AprovalProfile:Profile
    {
        public AprovalProfile()
        {
            CreateMap<ArticleApproval, aprovalDto>();
            CreateMap<CreateAproval, ArticleApproval>()
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
