using AutoMapper;
using TaskManagerAPI.Core.Models;
using TaskManagerAPI.Application.Models.DTOs.Task;
using TaskManagerAPI.Application.Models.DTOs.User;

namespace TaskManagerAPI.Application.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // User Mappings
            CreateMap<RegisterDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();

            // Task Mappings
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<TaskItem, TaskDto>();
            CreateMap<UpdateTaskDto, TaskItem>();
        }
    }
}
