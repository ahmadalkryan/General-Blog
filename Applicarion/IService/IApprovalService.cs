using Application.Dto.Approval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IApprovalService
    {
        Task<aprovalDto> CreateApproval(CreateAproval createAproval);

        Task<aprovalDto> GetApprovalByArticleId(int articleId);

        string GetStatusForArticle(int articleId);

        Task<IEnumerable<aprovalDto>> GetAllApprovals();



    }
}
