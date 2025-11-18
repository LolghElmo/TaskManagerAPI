using AutoMapper;
using TaskManagerAPI.Application.Features.Tasks;
using TaskManagerAPI.Application.Models.DTOs.Task;
using TaskManagerAPI.Application.Models.DTOs.User;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Application.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // User mappings
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<ApplicationUser, UserDto>();

            // Task mappings
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore());
            CreateMap<TaskItem, TaskDto>();

            // Command mappings
            CreateMap<CreateTaskCommand, TaskItem>();
            CreateMap<UpdateTaskCommand, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
