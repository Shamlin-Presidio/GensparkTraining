using FirstApi.Contexts;
using FirstApi.Interfaces;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;


namespace FirstApi.Repositories
{
    public  class AppointmentRepository : Repository<int, Appointment>
    {
        public AppointmentRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Appointment> Get(int key)
        {
            var appointments = await _clinicContext.Appointments.ToListAsync();
            var appointment = appointments.SingleOrDefault(p => int.TryParse(p.AppointmentNumber, out int num) && num == key);
            return appointment ?? throw new Exception("No appointments with the given ID");
        }


        public override async Task<IEnumerable<Appointment>> GetAll()
        {
            var appointments = _clinicContext.Appointments;
            if (appointments.Count() == 0)
                throw new Exception("No appointments in the database");
            return (await appointments.ToListAsync());
        }
    }
}