using Microsoft.AspNetCore.Http;

namespace EventManagementAPI.Interfaces;

public interface IBlobService    
{
    Task<string> UploadAsync(IFormFile file);
    Task<byte[]> DownloadAsync(string fileName);
}

