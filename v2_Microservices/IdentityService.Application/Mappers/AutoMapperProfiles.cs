using AutoMapper;
using IdentityService.Application.DTOs.Auth;
using IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Cryptography.X509Certificates;

namespace IdentityService.Application.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username)) 
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<User, UserDto>();
        }
    }
}
