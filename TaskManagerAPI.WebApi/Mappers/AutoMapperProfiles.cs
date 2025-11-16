using AutoMapper;
using TaskManagerAPI.Core.Models;
using TaskManagerAPI.WebApi.Models.DTOs.Task;
using TaskManagerAPI.WebApi.Models.DTOs.User;

namespace TaskManagerAPI.WebApi.Mappers
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
