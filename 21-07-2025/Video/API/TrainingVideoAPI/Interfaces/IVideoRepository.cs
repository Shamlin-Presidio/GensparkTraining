using TrainingVideoAPI.Models;
namespace TrainingVideoAPI.Interfaces;

public interface IVideoRepository
{
    Task AddAsync(TrainingVideo video);
    Task<List<TrainingVideo>> GetAllAsync();
    Task<TrainingVideo?> GetByIdAsync(int id);
}
