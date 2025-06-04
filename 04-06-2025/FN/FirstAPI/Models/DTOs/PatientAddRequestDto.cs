namespace FirstApi.Models.DTOs.Patients
{
    public class PatientAddRequestDto
    {
        public string Name { get; set; } = String.Empty;
        
        public int Age { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        
        public string Password { get; set; } = String.Empty;
    }
}