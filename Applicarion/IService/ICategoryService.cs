
using Applicaion.Dto.CategoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryByID(int id);

        Task<CategoryDto> CreateCategory( CreateCategoryDto createCategoryDto);
        Task<CategoryDto> UpdateCategory( UpdateCategoryDto updateCategoryDto);
        Task<CategoryDto> DeleteCategory(int id);
    }

}
