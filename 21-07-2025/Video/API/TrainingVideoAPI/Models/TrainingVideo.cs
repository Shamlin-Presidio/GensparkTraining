namespace TrainingVideoAPI.Models;

public class TrainingVideo
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime UploadDate { get; set; }
    public string BlobUrl { get; set; } = null!;
}
