using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FirstApi.Models;



namespace FirstApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        static List<Doctor> doctors = new List<Doctor> // dont use static, this is just for learning
        {
            new Doctor{Id=101,Name="Ramu"},
            new Doctor{Id=102,Name="Somu"},
        };
        [HttpGet] // GET
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            return Ok(doctors);
        }
        [HttpPost] // POST
        public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
        {
            doctors.Add(doctor);
            return Created("", doctor);
        }

        [HttpPut("{id}")] // PUT
        public ActionResult<Doctor> UpdateDoctor(int id, [FromBody] Doctor updatedDoctor)
        {
            var doctor = doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
                return NotFound($"Doctor with Id {id} not found.");

            doctor.Name = updatedDoctor.Name;
            return Ok(doctor);
        }

        [HttpDelete("{id}")] //Delete
        public ActionResult DeleteDoctor(int id)
        {
            var doctor = doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
                return NotFound($"Doctor with Id {id} not found.");

            doctors.Remove(doctor);
            return NoContent();
        }
    }
}
