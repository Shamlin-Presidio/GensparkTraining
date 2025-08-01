using Newtonsoft.Json;
using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _repository;
        private const string RecaptchaSecret = "6Le2GC8UAAAAAKzGJ7VQ3kIC6zqqbcWFpbp-l6Qv";

        public ContactUsService(IContactUsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContactUsResponseDto>> GetAllAsync()
        {
            var contacts = await _repository.GetAllAsync();
            return contacts.Select(c => new ContactUsResponseDto
            {
                ContactUsId = c.ContactUsId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Content = c.Content
            });
        }

        public async Task<ContactUsResponseDto?> GetByIdAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null) return null;

            return new ContactUsResponseDto
            {
                ContactUsId = contact.ContactUsId,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Content = contact.Content
            };
        }

        public async Task<ContactUsResponseDto> CreateAsync(ContactUsRequestDto dto)
        {
            if (!await IsCaptchaValid(dto.CaptchaResponse))
                throw new Exception("Captcha validation failed.");

            var contact = new ContactUs
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Content = dto.Content
            };

            await _repository.AddAsync(contact);
            await _repository.SaveAsync();

            return new ContactUsResponseDto
            {
                ContactUsId = contact.ContactUsId,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Content = contact.Content
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null) return false;

            _repository.Delete(contact);
            await _repository.SaveAsync();
            return true;
        }

        private async Task<bool> IsCaptchaValid(string captchaResponse)
        {
            using var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "secret", RecaptchaSecret },
                { "response", captchaResponse }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var result = await response.Content.ReadAsStringAsync();

            var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(result);
            return captchaResult != null && captchaResult.Success;
        }

        private class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; } = new();
        }
    }
}
