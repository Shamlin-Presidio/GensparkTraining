using Azure.Storage.Blobs;
using AzureBlobApi.Interfaces;
using Azure.Storage.Blobs.Models;



namespace AzureBlobApi.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration configuration)
        {
            var blobUri = configuration["AzureBlobSettings:BlobUri"];
            var sasToken = configuration["AzureBlobSettings:SasToken"];
            _containerClient = new BlobContainerClient(new Uri($"{blobUri}{sasToken}"));
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);
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
            var blobClient = _containerClient.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
                throw new FileNotFoundException("File not found in blob storage");

            var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            return ms.ToArray();
        }

    }
}
