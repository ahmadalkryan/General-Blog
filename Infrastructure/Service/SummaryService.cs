using Applicarion.Dto.Summary;
using Applicarion.IRepository;
using Applicarion.IService;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class SummaryService : ISummaryService
    {
        private readonly IRepository<ArticleSummary> _repo;
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly IAIService _aiService;

        public SummaryService(IMapper mapper,IArticleService articleService ,IRepository<ArticleSummary> repository ,IAIService aIService)
        {
            _mapper = mapper;
            _repo = repository;
            _aiService = aIService;
            _articleService = articleService;
        }
        public async Task<SummaryDto> CreateSummary(CreateSummaryDto createSummaryDto)
        {

            var summaryEntity = _mapper.Map<ArticleSummary>(createSummaryDto);
            var result = await _repo.Insertasync(summaryEntity);
            return _mapper.Map<SummaryDto>(result);


        }

        public async Task<SummaryDto> GetSummaryForArticle(int articleId)
        {
            var summary = await _repo.GetAllAsync();
            var result =  summary.Where(s => s._article.ID == articleId).FirstOrDefault();
            return _mapper.Map<SummaryDto>(result);

        }

         public async  Task<SummaryDto> GenerateSummaryForArticle(int articleId)
        {
            //var summary = await GetSummaryForArticle(articleId);
            //if (summary != null)
            //{
            //    return summary;
            //}
           

            var article = await _articleService.GetArticleByID(articleId);
            var summarizedText = await _aiService.SummarizeTextAsync(article.Content );
            var createSummaryDto = new CreateSummaryDto
            {
                Summary = summarizedText,
                articleId = articleId
            };
            return await CreateSummary( createSummaryDto);


        }
    }
}
