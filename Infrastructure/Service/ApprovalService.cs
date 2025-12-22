using Applicaion.IRepository;
using Applicaion.IService;
using Application.Dto.Approval;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class ApprovalService : IApprovalService
    {
        private readonly IRepository<ArticleApproval> _approvalRepository;
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;

        public ApprovalService(IRepository<ArticleApproval> approvalRepository, IMapper mapper ,IArticleService articleService)
        {
            _approvalRepository = approvalRepository;
            _mapper = mapper;
            _articleService = articleService;
        }

        public async Task<aprovalDto> CreateApproval(CreateAproval createAproval)
        {
            
            var res = _mapper.Map<ArticleApproval>(createAproval);
            var app = await GetApprovalByArticleId(createAproval._articleId);
            if(app == null)
            {
                await _approvalRepository.Insertasync(res);

                if (createAproval.Status == ApprovalStatus.Approved)
                {
                    await _articleService.ApproveArticle(createAproval._articleId);
                }
              

            }
            return _mapper.Map<aprovalDto>(res);

        }

        public async Task<aprovalDto> GetApprovalByArticleId(int articleId)
        {
            var res = await _approvalRepository.GetAllAsync();
            var approval = res.FirstOrDefault(a => a._articleId == articleId);
            return _mapper.Map<aprovalDto>(approval);

        }

        public async Task<IEnumerable<aprovalDto>> GetAllApprovals()
        {
           var result = await _approvalRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<aprovalDto>>(result);

        }

        public string GetStatusForArticle(int articleId)
        {
            var res = GetAllApprovals();
            var art = res.Result.Where(x => x.ID == articleId).FirstOrDefault();

            
            
            


                if (art !=null && art.Status == ApprovalStatus.Approved)
                {
                    return "Approved";
                }
                else if (art != null && art.Status == ApprovalStatus.Rejected)
                {
                    return "Rejected";
                }

            else
            {
                return "Pending";
            }

        }
    }
}
