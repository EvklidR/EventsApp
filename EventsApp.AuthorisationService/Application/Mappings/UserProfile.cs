using AutoMapper;
using EventsApp.AuthorisationService.Application.DTOs;
using EventsApp.AuthorisationService.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EventsApp.AuthorisationService.Application.Mappings
{
    public class UserProfileMapper : Profile
    {
        public UserProfileMapper()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => HashPassword(src.Password)));
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

}