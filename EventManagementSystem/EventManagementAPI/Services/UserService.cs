using AutoMapper;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models.DTOs.User;

namespace EventManagementAPI.Services;

public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IBlobService _blobService;

    public UserService(IUserRepository userRepository, IMapper mapper, IWebHostEnvironment env, IBlobService blobService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _env = env;
        _blobService = blobService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.IsDeleted)
                return null;
            return _mapper.Map<UserResponseDto>(user);
        }

        // public async Task<UserResponseDto> CreateUserAsync(UserCreateDto dto)
        // {
        //     var user = _mapper.Map<User>(dto);
        //     user.Id = Guid.NewGuid();
        //     user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        //     var createdUser = await _userRepository.AddAsync(user);
        //     return _mapper.Map<UserResponseDto>(createdUser);
        // }

        public async Task<UserResponseDto?> UpdateUserAsync(Guid id, UserUpdateDto dto)
        {

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.IsDeleted)
                return null;

            var validRoles = new[] { "Organizer", "Attendee" };
            if (!validRoles.Contains(dto.Role, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Role must be either 'Organizer' or 'Attendee'."); 
            }
            _mapper.Map(dto, user);


        if (dto.ProfilePicture != null)
        {
            // var folder = Path.Combine("UploadedFiles", "Users");
            // var extension = Path.GetExtension(dto.ProfilePicture.FileName);
            // var fileName = $"{user.Id}{extension}";
            // var folderPath = Path.Combine(_env.ContentRootPath, folder);
            // Directory.CreateDirectory(folderPath);
            // var filePath = Path.Combine(folderPath, fileName);

            // using var stream = new FileStream(filePath, FileMode.Create);
            // await dto.ProfilePicture.CopyToAsync(stream);

            // user.ProfilePicturePath = Path.Combine(folder, fileName).Replace("\\", "/");
                
                var extension = Path.GetExtension(dto.ProfilePicture.FileName);
                var blobFileName = $"Users/{user.Id}{extension}";
                var blobUrl = await _blobService.UploadAsync(dto.ProfilePicture, blobFileName);
                user.ProfilePicturePath = blobUrl;
            }

            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserResponseDto>(user);
        }

    public async Task<bool> DeleteUserAsync(Guid id, Guid currentUserId)
    {
        Console.WriteLine($"Requested ID: {id}, Current User ID: {currentUserId}");
        if (id != currentUserId)
            throw new UnauthorizedAccessException("You can only delete your own account.");

        return await _userRepository.DeleteAsync(id);
        }
    }
