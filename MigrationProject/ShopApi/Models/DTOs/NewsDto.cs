namespace ShopApi.Dtos
{
    public class NewsDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
    public class NewsCreateDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string? Image { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }

    public class NewsUpdateDto : NewsCreateDto
    {
        public int NewsId { get; set; }
    }

    public class NewsResponseDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string? Image { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string? Username { get; set; }
    }

}
