
using AutoMapper;
using FirstApi.Models;
using FirstApi.Models.DTOs.Patients;

public class PatientMapping : Profile
{
    public PatientMapping() 
    {
        // Map DTO to User
        CreateMap<PatientAddRequestDto, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email.ToLower())) // or however you assign username
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Set manually after encryption
            .ForMember(dest => dest.HashKey, opt => opt.Ignore())  // Set manually
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        // Map DTO to Patient
        CreateMap<PatientAddRequestDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // just once is enough
    }
}
