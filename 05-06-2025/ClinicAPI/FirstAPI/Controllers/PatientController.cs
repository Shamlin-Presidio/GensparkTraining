using System.Collections.Generic;
using System.Threading.Tasks;
using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: /api/Patient
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<Patient>>> GetAllPatients()
        {
            try
            {
                var patients = await _patientService.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving patients: {ex.Message}");
            }
        }

        // GET: /api/Patient/name/{name}
        [HttpGet("name/{name}")]
        [Authorize]
        public async Task<ActionResult<Patient>> GetPatientByName(string name)
        {
            try
            {
                var patient = await _patientService.GetPatientByName(name);
                if (patient == null)
                    return NotFound($"Patient with name '{name}' not found.");

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving patient: {ex.Message}");
            }
        }

        // POST: /api/Patient
        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] PatientAddRequestDto patientDto)
        {
            try
            {
                var newPatient = await _patientService.AddPatient(patientDto);
                return Created("", newPatient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}