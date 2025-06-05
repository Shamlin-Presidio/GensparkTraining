using docuNotify.Contexts;
using docuNotify.Interfaces;
using docuNotify.Models;

namespace docuNotify.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocuNotifyContext _context;

        public DocumentRepository(DocuNotifyContext context)
        {
            _context = context;
        }

        public void AddDocument(Document document) =>
            _context.Documents.Add(document);

        public void SaveChanges() =>
            _context.SaveChanges();
    }
}