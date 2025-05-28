using FirstApi.Contexts;
using FirstApi.Interfaces;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;


namespace FirstApi.Repositories
{
    public  class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
    {
        protected DoctorSpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<DoctorSpeciality> Get(int key)
        {
            var doctorSpeciality = await _clinicContext.DoctorSpecialities.SingleOrDefaultAsync(p => p.SerialNumber == key);

            return doctorSpeciality??throw new Exception("No Doctor Speciality with teh given ID");
        }

        public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
        {
            var doctorSpecialities = _clinicContext.DoctorSpecialities;
            if (doctorSpecialities.Count() == 0)
                throw new Exception("No Doctor Specialites in the database");
            return (await doctorSpecialities.ToListAsync());
        }
    }
}