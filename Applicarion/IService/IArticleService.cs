
using Applicaion.Dto.ArticleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaion.IService
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> GetAllArticles();
        Task<IEnumerable<ArticleDto>> GetApprovalArticles();
        Task<ArticleDto> GetArticleByID(int id);
        Task<IEnumerable<ArticleDto>> GetAll();
        Task<ArticleDto> CreateArticle(CrArticleDto createArticleDto );
        Task<ArticleDto> UpdateArticle(UpdateArticleDto updateArticleDto );
        Task<bool> ApproveArticle(int articleId);
        Task<ArticleDto> DeleteArticle(int id);
        Task<IEnumerable<ArticleDto>> FilterByCategory(int id);

        Task<IEnumerable<ArticleDto>> GetRejectArticle();
        Task<IEnumerable<ArticleDto>> GetPendingArticle();
        Task<IEnumerable<ArticleDto>> SearchArticle(string prompt);
    }
}
