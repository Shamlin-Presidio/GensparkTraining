using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckCardioApp.Interfaces;
using CheckCardioApp.Models;
using CheckCardioApp.Repositories;

namespace CheckCardioApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentRepository _repository = new();

        public int Add(Appointment appointment)
        {
            _repository.Add(appointment);
            return appointment.Id;
        }

        public void Update(Appointment appointment)
        {
            _repository.Update(appointment);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public Appointment? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public ICollection<Appointment> GetAll()
        {
            return _repository.GetAll();
        }

        public ICollection<Appointment> Search(AppointmentSearchModel model)
        {
            var results = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(model.PatientName))
            {
                results = results
                    .Where(a => a.PatientName.ToLower().Contains(model.PatientName.ToLower()))
                    .ToList();
            }

            if (model.AppointmentDate.HasValue)
            {
                results = results
                    .Where(a => a.AppointmentDate.Date == model.AppointmentDate.Value.Date)
                    .ToList();
            }

            if (model.AgeRange != null)
            {
                results = results
                    .Where(a => a.PatientAge >= model.AgeRange.MinVal && a.PatientAge <= model.AgeRange.MaxVal)
                    .ToList();
            }

            return results;
        }
    }

}
