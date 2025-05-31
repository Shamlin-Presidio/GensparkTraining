using BankingApi.Interfaces;

namespace BankingApi.Services
{
    public class FAQService : IFAQService
    {
        private readonly HttpClient _httpClient;

        public FAQService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAnswerAsync(string question)
        {
            var request = new { question };
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5001/faq", request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}