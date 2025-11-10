using Applicarion.Dto.ArticleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> GetAllArticles();
        Task<ArticleDto> GetArticleByID(int id);

        Task<ArticleDto> CreateArticle(CrArticleDto createArticleDto );
        Task<ArticleDto> UpdateArticle(UpdateArticleDto updateArticleDto );
        Task<ArticleDto> DeleteArticle(int id);
        Task<IEnumerable<ArticleDto>> FilterByCategory(int id);

        Task<IEnumerable<ArticleDto>> SearchArticle(string prompt);
    }
}
