using FirstApi.Models.DTOs;

namespace FirstApi.Interfaces
{
    public interface IOtherContextFunctionalities
    {
        public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity);
    }
}