using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckCardioApp.Models;

namespace CheckCardioApp.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        public override void Add(Appointment appointment)
        {
            appointment.Id = _nextId++;
            base.Add(appointment);
        }

        public override void Update(Appointment updated)
        {
            var existing = _items.FirstOrDefault(a => a.Id == updated.Id);
            if (existing != null)
            {
                existing.PatientName = updated.PatientName;
                existing.PatientAge = updated.PatientAge;
                existing.AppointmentDate = updated.AppointmentDate;
                existing.Reason = updated.Reason;
            }
        }

        public override void Delete(int id)
        {
            var appointment = _items.FirstOrDefault(a => a.Id == id);
            if (appointment != null)
            {
                _items.Remove(appointment);
            }
        }

        public override Appointment? GetById(int id)
        {
            return _items.FirstOrDefault(a => a.Id == id);
        }
    }
}
