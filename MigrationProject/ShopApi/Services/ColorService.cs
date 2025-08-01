using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public async Task<IEnumerable<ColorResponseDto>> GetAllAsync()
        {
            var colors = await _colorRepository.GetAllAsync();
            return colors.Select(c => new ColorResponseDto
            {
                ColorId = c.ColorId,
                Color = c.Name
            });
        }

        public async Task<ColorResponseDto?> GetByIdAsync(int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) return null;

            return new ColorResponseDto
            {
                ColorId = color.ColorId,
                Color = color.Name
            };
        }

        public async Task<ColorResponseDto> CreateAsync(ColorRequestDto dto)
        {
            var color = new Color { Name = dto.Color };
            await _colorRepository.AddAsync(color);
            await _colorRepository.SaveAsync();

            return new ColorResponseDto
            {
                ColorId = color.ColorId,
                Color = color.Name
            };
        }

        public async Task<ColorResponseDto?> UpdateAsync(int id, ColorRequestDto dto)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) return null;

            color.Name = dto.Color;
            _colorRepository.Update(color);
            await _colorRepository.SaveAsync();

            return new ColorResponseDto
            {
                ColorId = color.ColorId,
                Color = color.Name
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) return false;

            _colorRepository.Delete(color);
            await _colorRepository.SaveAsync();
            return true;
        }
    }
}
