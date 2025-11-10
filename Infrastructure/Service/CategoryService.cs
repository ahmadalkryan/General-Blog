using Applicarion.Dto.CategoryDto;
using Applicarion.Dto.CommentDto;
using Applicarion.IRepository;
using Applicarion.IService;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly IRepository<Category> _repository;
        private readonly IMapper _mapper;
       

        public CategoryService(IMapper mapper, IRepository<Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
            

        }
        public async Task<CategoryDto> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var cat = _mapper.Map<Category>(createCategoryDto);
            await _repository.Insertasync(cat);
            return _mapper.Map<CategoryDto>(cat);
        }

        public async Task<CategoryDto> DeleteCategory(int id)
        {
            var cat = await _repository.GetById(id);

            await _repository.RemoveAsync(cat);

            return _mapper.Map<CategoryDto>(cat);

        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories() => _mapper.Map<IEnumerable<CategoryDto>>(await _repository.GetAllAsync());



        public async Task<CategoryDto> GetCategoryByID(int id)
        {
            return _mapper.Map<CategoryDto>(await _repository.GetById(id));
        }

        public async Task<CategoryDto> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
             var cat = _mapper.Map<Category>(updateCategoryDto);

            await _repository.UpdateAsync(cat);

            return _mapper.Map<CategoryDto>(cat);
        }
    }
}
