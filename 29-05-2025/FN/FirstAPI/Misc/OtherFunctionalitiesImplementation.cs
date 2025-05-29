
using FirstApi.Models.DTOs;
using FirstApi.Contexts;
using FirstApi.Interfaces;


namespace FirstAFirstApi.Misc
{
    public class OtherFuncinalitiesImplementation : IOtherContextFunctionalities
    {
        private readonly ClinicContext _clinicContext;

        public OtherFuncinalitiesImplementation(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity)
        {
            var result = await _clinicContext.GetDoctorsBySpeciality(specilaity);
            return result;
        }
    }
}
