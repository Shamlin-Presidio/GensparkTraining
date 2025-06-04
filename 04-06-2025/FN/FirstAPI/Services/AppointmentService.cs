using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs;

namespace FirstApi.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<string, Appointment> _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IRepository<string, Appointment> appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<Appointment> AddAppointment(AppointmentAddRequestDto appointmentDto)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(appointmentDto);
                appointment.AppointmentNumber = Guid.NewGuid().ToString(); // Generate a unique ID
                var result = await _appointmentRepository.Add(appointment);

                if (result == null)
                    throw new Exception("Could not create appointment.");

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Error creating appointment: {e.Message}");
            }
        }

        
        public async Task<bool> DeleteAppointment(string appointmentNumber)
        {
            try
            {
                var deletedAppointment = await _appointmentRepository.Delete(appointmentNumber);
                return deletedAppointment != null;
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting appointment: {e.Message}");
            }
        }


    }
}