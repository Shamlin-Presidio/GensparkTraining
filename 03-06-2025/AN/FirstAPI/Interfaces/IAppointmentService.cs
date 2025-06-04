using FirstApi.Models;
using FirstApi.Models.DTOs;


namespace FirstApi.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> AddAppointment(AppointmentAddRequestDto appointmentDto);
        Task<bool> DeleteAppointment(string appointmentNumber);
    }
}