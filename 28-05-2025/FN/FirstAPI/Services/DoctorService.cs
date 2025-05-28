using FirstApi.Interfaces;
using FirstApi.Models.DTOs;
using FirstApi.Models;

namespace FirstApi.Services
{
    public class DoctorService : IDoctorService
    {
        // Repos
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;


        public DoctorService(IRepository<int, Doctor> doctorRepository,
                            IRepository<int, Speciality> specialityRepository,
                            IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }

        // A D D    D O C T O R   
        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience,
                Status = "Active",
                DoctorSpecialities = new List<DoctorSpeciality>()
            };

            var createdDoctor = await _doctorRepository.Add(doctor);

            if (doctorDto.Specialities != null)
            {
                foreach (var specDto in doctorDto.Specialities)
                {
                    var speciality = (await _specialityRepository.GetAll())
                        .FirstOrDefault(s => s.Name.ToLower() == specDto.Name.ToLower());

                    if (speciality == null)
                    {
                        speciality = await _specialityRepository.Add(new Speciality { Name = specDto.Name });
                    }

                    var doctorSpeciality = await _doctorSpecialityRepository.Add(new DoctorSpeciality
                    {
                        DoctorId = createdDoctor.Id,
                        SpecialityId = speciality.Id,
                        Speciality = speciality
                    });


                    createdDoctor.DoctorSpecialities.Add(doctorSpeciality);
                }
            }

        return createdDoctor;
    }



        // G E T   DOCTOR By name.....

        public async Task<Doctor> GetDoctByName(string name)
        {
            var allDoctors = await _doctorRepository.GetAll();
            return allDoctors.FirstOrDefault(d => d.Name.ToLower() == name.ToLower());
        }


        // G E T  ALL DOCTORS By their speciality..... (doctors specialied in cardio, ortho etc... )

        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            var specialities = await _specialityRepository.GetAll();
            var spec = specialities.FirstOrDefault(s => s.Name.ToLower() == speciality.ToLower());

            if (spec == null) return new List<Doctor>();

            var doctorSpecialities = await _doctorSpecialityRepository.GetAll();
            var doctorIds = doctorSpecialities
                .Where(ds => ds.SpecialityId == spec.Id)
                .Select(ds => ds.DoctorId)
                .ToList();

            var allDoctors = await _doctorRepository.GetAll();
            return allDoctors.Where(d => doctorIds.Contains(d.Id)).ToList();
        }

    }
}