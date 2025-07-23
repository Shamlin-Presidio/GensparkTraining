namespace TrainingVideoAPI.DTOs;

public class VideoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string BlobUrl { get; set; } = null!;
}