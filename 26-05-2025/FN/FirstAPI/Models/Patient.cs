public class Patient
{
    public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int Age { get; set; }

        public DateTime DateOfDiagnosis { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public float Temperature { get; set; }

        public string BloodPressure { get; set; } = string.Empty;
}