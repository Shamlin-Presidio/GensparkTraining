using docuNotify.Models;

namespace docuNotify.Interfaces
{
    public interface IDocumentRepository
    {
        void AddDocument(Document document); 
        void SaveChanges();
    }
}