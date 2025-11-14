using Applicarion.Dto.Summary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface ISummaryService
    {
        Task<SummaryDto> CreateSummary(CreateSummaryDto createSummaryDto);

        Task<SummaryDto> GetSummaryForArticle(int articleId);

       Task<SummaryDto> GenerateSummaryForArticle(int articleId);
    }
}
