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
            // User Mappings
            CreateMap<RegisterDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();

            // Task Mappings
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<TaskItem, TaskDto>();
            CreateMap<UpdateTaskDto, TaskItem>();

            // Command Mappings
            CreateMap<CreateTaskCommand, TaskItem>();
        }
    }
}
