using Applicaion.Dto.Summary;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class SummaryProfile:AutoMapper.Profile
    {
        public SummaryProfile()
        {
            CreateMap<CreateSummaryDto,ArticleSummary>();
            CreateMap<ArticleSummary, SummaryDto>();
        }
    }
}
