using Azure.Identity;

using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventManagementAPI.Interfaces;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _container;

    public BlobService(IConfiguration configuration)
    {
        // var blobUri = config["AzureBlobSettings:BlobUri"];
        // var sasToken = config["AzureBlobSettings:SasToken"];
        // _container = new BlobContainerClient(new Uri($"{blobUri}{sasToken}"));

        var vault = configuration["AzureVaultSettings:VaultUri"];
        var keyName = "BlobURL";
        var token = "SasToken";

        SecretClient secretClient = new SecretClient(new Uri(vault), new DefaultAzureCredential());

        KeyVaultSecret secret = secretClient.GetSecret(keyName);
        KeyVaultSecret vaultToken = secretClient.GetSecret(token);

        string sasToken = vaultToken.Value;
        string blobUri = secret.Value;

        _container = new BlobContainerClient(new Uri($"{blobUri}{sasToken}"));
    }

    public async Task<string> UploadAsync(IFormFile file, string blobFileName)
        {
            var blobClient = _container.GetBlobClient(blobFileName);
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
