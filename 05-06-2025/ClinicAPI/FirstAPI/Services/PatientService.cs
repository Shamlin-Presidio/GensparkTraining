using System.Threading.Tasks;
using AutoMapper;
using FirstApi.Interfaces;
using FirstApi.Misc;
using FirstApi.Models;
using FirstApi.Models.DTOs.Patients;

namespace FirstApi.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public PatientService(IRepository<int, Patient> patientRepository,
                              IRepository<string, User> userRepository,
                              IEncryptionService encryptionService,
                              IMapper mapper)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _mapper = mapper;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDto patient)
        {
            try
            {
                var user = _mapper.Map<PatientAddRequestDto, User>(patient);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = patient.Password
                });
                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;
                user.Role = "Patient";
                user = await _userRepository.Add(user);

                var newPatient = _mapper.Map<PatientAddRequestDto, Patient>(patient);
                
                newPatient.UserId = user.Username;
                newPatient = await _patientRepository.Add(newPatient);

                if (newPatient == null)
                    throw new Exception("Could not add patient");

                return newPatient;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Patient> GetPatientByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Patient>> GetAllPatients()
        {
            try
            {
                var patients =  await _patientRepository.GetAll();
                return patients.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving patients: {e.Message}");
            }
        }
    }
}