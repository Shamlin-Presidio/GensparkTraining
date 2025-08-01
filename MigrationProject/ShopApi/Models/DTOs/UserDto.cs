namespace ShopApi.Dtos
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
    }

    public class UserCreateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
    }

    public class UserUpdateDto
    {
        public string? FullName { get; set; }
    }
}
