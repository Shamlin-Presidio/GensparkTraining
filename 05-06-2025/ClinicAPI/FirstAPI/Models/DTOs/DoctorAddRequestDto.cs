using FirstApi.Misc; 
using System.ComponentModel.DataAnnotations;

namespace FirstApi.Models.DTOs
{
    public class DoctorAddRequestDto
    {
        [NameValidation(ErrorMessage = "Name must only contain letters and spaces.")]
        public string Name { get; set; } = string.Empty;
        public ICollection<SpecialityAddRequestDto>? Specialities { get; set; }
        public float YearsOfExperience { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}