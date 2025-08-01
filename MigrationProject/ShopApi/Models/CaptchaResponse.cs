namespace ShopApi.Models
{
    public class CaptchaResponse
    {
        public bool Success { get; set; }

        public string Challenge_ts { get; set; }

        public string Hostname { get; set; }

        public string[] ErrorCodes { get; set; }
    }
}
