using System.Threading.Tasks;
using FirstApi.Interfaces;
using FirstApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentAddRequestDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var appointment = await _appointmentService.AddAppointment(appointmentDto);
                return CreatedAtAction(nameof(GetAppointment), new { appointmentNumber = appointment.AppointmentNumber }, appointment);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error creating appointment: {ex.Message}");
            }
        }
        
        [HttpDelete("{appointmentNumber}")]
        [Authorize(Policy = "DoctorExperienceCheck")]
        public async Task<IActionResult> DeleteAppointment(string appointmentNumber)
        {
            try
            {
                var result = await _appointmentService.DeleteAppointment(appointmentNumber);
                if (!result)
                    return NotFound("Appointment not found.");

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error deleting appointment: {ex.Message}");
            }
        }
        
        [HttpGet("{appointmentNumber}")]
        [Authorize]
        public IActionResult GetAppointment(string appointmentNumber)
        {
            return Ok($"Returning appointment {appointmentNumber} (stub)");
        }
    }
}