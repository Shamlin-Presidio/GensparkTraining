
using AutoMapper;
using FirstApi.Models;
using FirstApi.Models.DTOs;

namespace FirstApi.Misc;

public class AppointmentMapper : Profile
{
    public AppointmentMapper()
    {
        CreateMap<AppointmentAddRequestDto, Appointment>();
    }
}
