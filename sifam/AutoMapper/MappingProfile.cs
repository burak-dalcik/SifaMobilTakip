using AutoMapper;
using sifam.DTOs;
using sifam.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sifam.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();

            CreateMap<AppointmentDTO, Appointment>();

        }
    }
}
