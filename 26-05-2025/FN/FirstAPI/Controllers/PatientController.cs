using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FirstApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class PatientController : ControllerBase
    {
        static List<Patient> patients = new List<Patient> // for learning only
        {
            new Patient { Id = 201, Name = "Umar" }, // doctor = Ramu, Patient = Umar   :)
            new Patient { Id = 202, Name = "Umos" }, // doctor = Somu, Patient = Umos   :)
        };

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            return Ok(patients);
        }

        [HttpPost]
        public ActionResult<Patient> PostPatient([FromBody] Patient patient)
        {
            patients.Add(patient);
            return Created("", patient);
        }

        [HttpPut("{id}")]
        public ActionResult<Patient> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound($"Patient with Id {id} not found.");

            patient.Name = updatedPatient.Name;
            return Ok(patient);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePatient(int id)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound($"Patient with Id {id} not found.");

            patients.Remove(patient);
            return NoContent();
        }
    }
}