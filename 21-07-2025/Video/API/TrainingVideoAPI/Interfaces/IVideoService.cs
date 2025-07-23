using TrainingVideoAPI.DTOs;

namespace TrainingVideoAPI.Interfaces;

public interface IVideoService
{
    Task<VideoResponseDto> UploadVideoAsync(VideoUploadDto dto);
    Task<List<VideoResponseDto>> GetAllVideosAsync();
    Task<Stream> GetVideoStreamAsync(int videoId);
}
