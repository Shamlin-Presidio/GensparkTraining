using AutoMapper;
using ShopApi.Dtos;
using ShopApi.Interfaces;
using ShopApi.Models;
using ShopApi.Services.Interfaces;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);
            // optionally hash password
            var created = await _repo.AddAsync(user);
            return _mapper.Map<UserResponseDto>(created);
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            user.FullName = dto.FullName;
            return await _repo.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repo.DeleteAsync(id);
    }
}
