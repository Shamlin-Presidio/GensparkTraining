namespace docuNotify.Interfaces
{
    public interface IDocumentService
    {
        Task<string> UploadDocumentAsync(IFormFile file, IWebHostEnvironment env);
    }
}