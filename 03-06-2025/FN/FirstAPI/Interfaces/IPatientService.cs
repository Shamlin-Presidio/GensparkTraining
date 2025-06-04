using FirstApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstApi.Models.DTOs.Patients;

namespace FirstApi.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> AddPatient(PatientAddRequestDto patient);
        Task<Patient> GetPatientByName(string name);
        Task<ICollection<Patient>> GetAllPatients();
    }
}