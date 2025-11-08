using AutoMapper;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTOs.Task;
using TaskManagerAPI.Models.DTOs.User;

namespace TaskManagerAPI.Utility
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
        }
    }
}
