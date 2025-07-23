namespace TrainingVideoAPI.DTOs;

public class VideoUploadDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile VideoFile { get; set; } = null!;
}