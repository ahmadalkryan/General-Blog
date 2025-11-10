using Applicarion.Dto.ArticleDto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Mapper
{
    public class ArticleProfile:Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>().
                ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src._user.UserName)).
                ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src._category.CategoryName));

            CreateMap<CreateArticleDto, Article>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore()) // تجاهل في البداية
            .AfterMap((src, dest) => {
                if (src.Image != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(src.Image.FileName);
                    dest.ImageUrl = fileName; // تعيين المسار هنا
                  
                }
                else
                {
                    dest.ImageUrl = " ";
                }
            });


            CreateMap<UpdateArticleDto, Article>().
                ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)).ForMember(dest => dest.ImageUrl, opt => opt.Ignore()) // تجاهل في البداية
            .AfterMap((src, dest) =>
            {
                if (src.Image != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(src.Image.FileName);
                   // dest.ImageUrl =  fileName;
                    dest.ImageUrl = "/Images/Articles/" + fileName;  // تعيين المسار هنا
                }
                else
                {
                    dest.ImageUrl = " ";
                }
            });










        }
    }
}
