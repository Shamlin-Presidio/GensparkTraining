using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckCardioApp.Models;

namespace CheckCardioApp.Interfaces
{
    public interface IAppointmentService
    {
        int Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int id);
        ICollection<Appointment> Search(AppointmentSearchModel model);
        Appointment? GetById(int id);
        ICollection<Appointment> GetAll();
    }
}
