using Azure.Storage.Blobs;
using TrainingVideoAPI.DTOs;
using TrainingVideoAPI.Models;
using TrainingVideoAPI.Interfaces;

namespace TrainingVideoAPI.Services;

// public class VideoService : IVideoService
// {
//     private readonly IVideoRepository _repository;
//     private readonly IConfiguration _config;

//     public VideoService(IVideoRepository repository, IConfiguration config)
//     {
//         _repository = repository;
//         _config = config;
//     }

//     public async Task<VideoResponseDto> UploadVideoAsync(VideoUploadDto dto)
//     {
//         // var containerName = _config["AzureBlobSettings:ContainerName"];
//         // var connectionString = _config["AzureBlobSettings:ConnectionString"];
//         var sasUrl = _config["AzureBlobSettings:SasURL"];


//         // var blobServiceClient = new BlobServiceClient(connectionString);
//         // var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
//         // await containerClient.CreateIfNotExistsAsync();

//         var containerClient = new BlobContainerClient(new Uri(sasUrl));


//         var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}_{dto.VideoFile.FileName}");
//         using var stream = dto.VideoFile.OpenReadStream();
//         await blobClient.UploadAsync(stream, overwrite: true);

//         var video = new TrainingVideo
//         {
//             Title = dto.Title,
//             Description = dto.Description,
//             UploadDate = DateTime.UtcNow,
//             BlobUrl = blobClient.Uri.ToString()
//         };

//         await _repository.AddAsync(video);

//         return new VideoResponseDto
//         {
//             Id = video.Id,
//             Title = video.Title,
//             Description = video.Description,
//             BlobUrl = video.BlobUrl
//         };
//     }

//     public async Task<List<VideoResponseDto>> GetAllVideosAsync()
//     {
//         var videos = await _repository.GetAllAsync();
//         return videos.Select(v => new VideoResponseDto
//         {
//             Id = v.Id,
//             Title = v.Title,
//             Description = v.Description,
//             BlobUrl = v.BlobUrl
//         }).ToList();
//     }
//     public async Task<Stream> GetVideoStreamAsync(string blobName)
//     {
//         var containerName = _config["AzureBlobSettings:ContainerName"];
//         var sasUrl = _config["AzureBlobSettings:SasURL"];

//         // Create container client using the SAS URL
//         var containerClient = new BlobContainerClient(new Uri(sasUrl));

//         var blobClient = containerClient.GetBlobClient(blobName);

//         if (!await blobClient.ExistsAsync())
//             throw new FileNotFoundException("Video not found in blob storage.");

//         var downloadResponse = await blobClient.DownloadStreamingAsync();
//         return downloadResponse.Value.Content;
//     }

// }

public class VideoService : IVideoService
{
    private readonly IVideoRepository _repository;
    private readonly IConfiguration _config;

    public VideoService(IVideoRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    public async Task<VideoResponseDto> UploadVideoAsync(VideoUploadDto dto)
    {
        var sasUrl = _config["AzureBlobSettings:SasURL"];
        var containerClient = new BlobContainerClient(new Uri(sasUrl));

        var blobName = $"{Guid.NewGuid()}_{dto.VideoFile.FileName}";
        var blobClient = containerClient.GetBlobClient(blobName);

        using var stream = dto.VideoFile.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        var video = new TrainingVideo
        {
            Title = dto.Title,
            Description = dto.Description,
            UploadDate = DateTime.UtcNow,
            BlobUrl = blobClient.Uri.ToString() // includes blob name
        };

        await _repository.AddAsync(video);

        return new VideoResponseDto
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            BlobUrl = video.BlobUrl
        };
    }

    public async Task<List<VideoResponseDto>> GetAllVideosAsync()
    {
        var videos = await _repository.GetAllAsync();
        return videos.Select(v => new VideoResponseDto
        {
            Id = v.Id,
            Title = v.Title,
            Description = v.Description,
            BlobUrl = v.BlobUrl
        }).ToList();
    }

    public async Task<Stream> GetVideoStreamAsync(int videoId)
    {

        var video = await _repository.GetByIdAsync(videoId);
        if (video == null)
            throw new KeyNotFoundException("Video metadata not found.");


        var blobUrl = new Uri(video.BlobUrl);
        var blobName = Path.GetFileName(blobUrl.LocalPath);

        var sasUrl = _config["AzureBlobSettings:SasURL"];
        var containerClient = new BlobContainerClient(new Uri(sasUrl));
        var blobClient = containerClient.GetBlobClient(blobName);


        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException("Video not found in blob storage.");

        var downloadResponse = await blobClient.DownloadStreamingAsync();
        return downloadResponse.Value.Content;
    }
}
