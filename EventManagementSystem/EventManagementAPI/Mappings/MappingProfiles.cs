using AutoMapper;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.User;
using EventManagementAPI.Models.DTOs.Event;
using EventManagementAPI.Models.DTOs.Registration;

namespace EventManagementAPI.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<User, UserResponseDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();


            CreateMap<Event, EventResponseDto>()
                .ForMember(dest => dest.OrganizerName,
                           opt => opt.MapFrom(src => src.Organizer.Username));

            CreateMap<EventCreateDto, Event>();
            CreateMap<EventUpdateDto, Event>();


            CreateMap<Registration, RegistrationResponseDto>()
                .ForMember(dest => dest.EventTitle,
                           opt => opt.MapFrom(src => src.Event.Title));

            CreateMap<RegistrationCreateDto, Registration>();
        }
    }
}
