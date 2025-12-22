using Applicaion.Dto.CategoryDto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profile = AutoMapper.Profile;

namespace Application.Mapper
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category , CategoryDto>  ();
             CreateMap< CreateCategoryDto, Category> ();
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
