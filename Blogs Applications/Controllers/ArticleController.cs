using Applicarion.Dto.ArticleDto;
using Applicarion.IService;
using Application.Dtos.Action;
using Application.Serializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IJsonFieldsSerializer _josnFieldSeriliezer;
        public ArticleController(IArticleService articleService ,IJsonFieldsSerializer jsonFieldsSerializer
            ,IWebHostEnvironment webHostEnvironment )
        {
            _articleService = articleService;
            _webHostEnvironment = webHostEnvironment;
            _josnFieldSeriliezer = jsonFieldsSerializer;
            
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> InsertArticle([FromForm] CrArticleDto dto)
        {

            if (dto.Image != null)
            {
                string wwwRootPAth = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                string directoryPath = Path.Combine(wwwRootPAth, "Images", "Articles");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filePath = Path.Combine(directoryPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(fileStream);
                }
              

            }

            // id to service to create article after mapper 

        

            var result = await _articleService.CreateArticle(dto);
            if (result == null)
            {
                return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                    new ApiResponse(false, "failed created ", StatusCodes.Status400BadRequest), string.Empty));
                    
            }
            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                new ApiResponse(true, "Article Created successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ArticleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> GetAllArticle()
        {

            var result = await _articleService.GetAllArticles();

            //  if(result == null)
            //{
            //    return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
            //        new ApiResponse(false, "failed loaded ", StatusCodes.Status400BadRequest), string.Empty));

            //}
            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                new ApiResponse(true, "Article loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ArticleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticleByCategoryName([FromQuery] int id)
        {

            var result = await _articleService.FilterByCategory(id);

            if (result == null)
            {
                return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                    new ApiResponse(false, "failed loaded ", StatusCodes.Status400BadRequest), string.Empty));

            }
            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                new ApiResponse(true, "Article loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ArticleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> searchArticle(string prompt)
        {

            var result = await _articleService.SearchArticle(prompt);

            if (result == null)
            {
                return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                    new ApiResponse(false, "failed loaded ", StatusCodes.Status400BadRequest), string.Empty));

            }
            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                new ApiResponse(true, "Article loaded successfully", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpGet]

        public async Task<IActionResult> GetArticleByID([FromQuery]int Id)
        {
            var result = await _articleService.GetArticleByID(Id);

            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                new ApiResponse(true , "", StatusCodes.Status200OK , result), string.Empty)); 
        }

        [HttpPut]

        public async Task<IActionResult> UpdateArticle([FromBody]UpdateArticleDto updateArticleDto)
        {

            var result = await _articleService.UpdateArticle(updateArticleDto);

            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
                new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArticle([FromQuery]int id)
        {
            var result = await _articleService.DeleteArticle(id);
            return new RawJsonActionResult(_josnFieldSeriliezer.Serialize(
               new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }


    }
}
