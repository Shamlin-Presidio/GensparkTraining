using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventManagementAPI.Interfaces;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _container;

    public BlobService(IConfiguration config)
    {
        var blobUri = config["AzureBlobSettings:BlobUri"];
        var sasToken = config["AzureBlobSettings:SasToken"];
        _container = new BlobContainerClient(new Uri($"{blobUri}{sasToken}"));
    }

    public async Task<string> UploadAsync(IFormFile file)
        {
            var blobClient = _container.GetBlobClient(file.FileName);
            await using var stream = file.OpenReadStream();
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType 
            };

            await blobClient.UploadAsync(file.OpenReadStream(), new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            // await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        } 

    public async Task<byte[]> DownloadAsync(string fileName)
    {
        var blob = _container.GetBlobClient(fileName);
        var ms = new MemoryStream();
        await blob.DownloadToAsync(ms);
        return ms.ToArray();
    }
}
