using Applicaion.Dto.Summary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaion.IService
{
    public interface ISummaryService
    {
        Task<SummaryDto> CreateSummary(CreateSummaryDto createSummaryDto);

        Task<string> GetModelInfoAsync();
        Task<SummaryDto> GetSummaryForArticle(int articleId);

       Task<SummaryDto> GenerateSummaryForArticle(int articleId);
    }
}
