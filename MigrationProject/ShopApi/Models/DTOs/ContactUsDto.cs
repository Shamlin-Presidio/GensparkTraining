
namespace ShopApi.Dtos
{
    public class ContactUsRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public string CaptchaResponse { get; set; }
    }
    public class ContactUsResponseDto
    {
        public int ContactUsId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
    }
}