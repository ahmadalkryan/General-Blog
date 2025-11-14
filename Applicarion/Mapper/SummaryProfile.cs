using Applicarion.Dto.Summary;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Mapper
{
    public class SummaryProfile:Profile
    {
        public SummaryProfile()
        {
            CreateMap<CreateSummaryDto,ArticleSummary>();
            CreateMap<ArticleSummary, SummaryDto>();
        }
    }
}
