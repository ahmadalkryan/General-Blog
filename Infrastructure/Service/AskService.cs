



using Applicaion.IService;
using Applicaion.Dto.ArticelQuestions;
using Application.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class AskService : IAskService
    {
        private readonly IArticleService _articleService;
        private readonly IAIService _aiService;


        public AskService(IArticleService articleService, IAIService aIService)
        {
            _aiService = aIService;

            _articleService = articleService;
        }

        public async Task<string> AskQuestion(CreateAnswer createAnswer)
        {
            var article = await _articleService.GetArticleByID(createAnswer.articleId);

            var result = _aiService.GenerateAnswerAsync(article.Content, createAnswer.question);

            return await result;

        }

        
    }
}
