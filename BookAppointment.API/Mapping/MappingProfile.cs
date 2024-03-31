using AutoMapper;
using BookAppointment.API.DTOs.Request;
using BookAppointment.API.DTOs.Response;
using BookAppointment.Core.DTOs;
using BookAppointment.Core.Entities;

namespace BookAppointment.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<RegisterCustomerRequest, Customer>();
            CreateMap<RegisterRequest, AgencyUser>();
            CreateMap<BookAppointmentRequest, Appointment>();
            CreateMap<Appointment, BookAppointmentResponse>()
                .ForMember(d => d.CustomerId, d => d.MapFrom(src => src.CustomerId))
                .ForMember(d => d.AppointmentDate, d => d.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd")))
                .ForMember(d => d.AppointmentTime, d => d.MapFrom(src => src.AppointmentTime.ToString(@"hh\:mm")))
                .ForMember(d => d.Token, d => d.MapFrom(src => src.Token))
                .ForMember(d => d.CreatedAt, d => d.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")));
            CreateMap<Appointment, AppointmentView>()
                .ForMember(d => d.CustomerName, d => d.MapFrom(src => src.Customer.FullName))
                .ForMember(d => d.Token, d => d.MapFrom(src => src.Token))
                .ForMember(d => d.AppointmentDate, d => d.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd")))
                .ForMember(d => d.AppointmentTime, d => d.MapFrom(src => src.AppointmentTime.ToString(@"hh\:mm")));
            CreateMap<DayoffRequest, DayOff>();

        }
    }
}
