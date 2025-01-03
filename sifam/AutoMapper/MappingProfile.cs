using AutoMapper;
using sifam.DTOs;
using sifam.Models;

namespace sifam.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient to PatientDTO mapping
            CreateMap<Patient, PatientDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();

            // AppointmentDTO to Appointment mapping
            CreateMap<AppointmentCreateDto, Appointment>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ReverseMap();
        }
    }
}
