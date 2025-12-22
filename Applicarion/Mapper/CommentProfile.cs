using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using Domain.Entities;
using Applicaion.Dto.CommentDto;


namespace Application.Mapper
{
    public class CommentProfile:AutoMapper.Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>().ForMember(
                dest => dest.UserName, opt => opt.MapFrom(src => src._user.UserName)

                ).ForMember(dest => dest.articleTitle,
                opt => opt.MapFrom(src => src._article.Title));

            CreateMap<CreateCommentDto ,Comment>()
                .ForMember(dest =>dest.CreatedAt ,opt=>opt.MapFrom(src=>DateTime.UtcNow));

            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        }
    }
}
