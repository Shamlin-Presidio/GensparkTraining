using FirstApi.Models;
using FirstApi.Models.DTOs;

namespace FirstApi.Misc
{
    public class DoctorMapper
    {
        public Doctor? MapDoctorAddRequestDoctor(DoctorAddRequestDto addRequestDto)
        {
            Doctor doctor = new();
            doctor.Name = addRequestDto.Name;
            doctor.YearsOfExperience = addRequestDto.YearsOfExperience;
            return doctor;
        }
    }
}