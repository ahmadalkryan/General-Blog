using Applicarion.Dto.ArticleDto;
using Applicarion.Dto.CategoryDto;
using Applicarion.Dto.UserDto;
using Applicarion.IService;
using Application.Dtos.Action;
using Application.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blogs_Applications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        private readonly IJsonFieldsSerializer _jsonFieldsSerializer;

        public CategoryController(IJsonFieldsSerializer jsonFieldsSerializer ,ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _jsonFieldsSerializer = jsonFieldsSerializer;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> InsertCategory([FromForm] CreateCategoryDto dto)
        {
            var result  =await _categoryService.CreateCategory(dto);
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
              new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpGet]

        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _categoryService.GetAllCategories();
            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));

        }

        [HttpGet]

        public async Task<IActionResult> GetCategory([FromQuery] int id)
        {
            var result = await _categoryService.GetCategoryByID(id);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpPut]

        public async Task<IActionResult> UpdateCategory([FromForm]UpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryService.UpdateCategory(updateCategoryDto);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }


        [HttpDelete]

        public async Task<IActionResult> DeleteCategory([FromQuery] int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            return new RawJsonActionResult(_jsonFieldsSerializer.Serialize(
             new ApiResponse(true, "", StatusCodes.Status200OK, result), string.Empty));
        }





        }
}
