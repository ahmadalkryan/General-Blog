using Application.Dto.Approval;
using Application.Dtos.Action;
using Application.IService;
using Application.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvalService;
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;
        public ApprovalController(IApprovalService approvalService ,IJsonFieldsSerializer jsonFieldsSerializer)
        {
            _jsonFieldsSerializer = jsonFieldsSerializer;
            _approvalService = approvalService;
        }

        [HttpPost]

        public async Task<IActionResult> CreateApproval([FromBody] CreateAproval createAproval)
        {
            var result = await _approvalService.CreateApproval(createAproval);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Article loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]

        public async Task<IActionResult> GetApprovalByArticleId([FromQuery] int articleId)
        {
            var result = await _approvalService.GetApprovalByArticleId(articleId);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Approval loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]

        public async Task<IActionResult> GetAllApprovall()
        {
            var result = await _approvalService.GetAllApprovals();
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Approval loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]

        public async Task<IActionResult> GetStatusForArticel(int articleId)
        {
            var result =  _approvalService.GetStatusForArticle(articleId);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
                new ApiResponse(true, "Approval loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }




    }
}
