using AutoMapper;
using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.CategoryId);
            if (existing == null) return false;

            existing.Name = dto.Name;
            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}
