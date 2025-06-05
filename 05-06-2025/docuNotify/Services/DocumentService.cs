using docuNotify.Hubs;
using docuNotify.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace docuNotify.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IHubContext<DocumentHub> _hubContext;
        private readonly IWebHostEnvironment _env;

        public DocumentService(IDocumentRepository documentRepository,
            IHubContext<DocumentHub> hubContext,
            IWebHostEnvironment env)
        {
            _documentRepository = documentRepository;
            _hubContext = hubContext;
            _env = env;
        }

        public async Task<string> UploadDocumentAsync(IFormFile file, IWebHostEnvironment env)
        {
            var folderPath = Path.Combine(_env.ContentRootPath, "UploadedFiles");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, file.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            await _hubContext.Clients.All.SendAsync("NewDocumentUploaded", file.FileName);

            return file.FileName;
        }
    }

}