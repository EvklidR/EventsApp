using AutoMapper;
using AuthorisationService.Application.DTOs;
using AuthorisationService.Domain.Entities;


namespace AuthorisationService.Application.Mappings
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