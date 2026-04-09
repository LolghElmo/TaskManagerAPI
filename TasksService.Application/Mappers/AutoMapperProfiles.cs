using AutoMapper;
using TasksService.Application.DTOs;
using TasksService.Domain.Models;
using TasksService.Domain.Models.Enums; // Ensure you have this namespace

namespace TasksService.Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TodoItem, TodoDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));

            CreateMap<CreateTodoRequest, TodoItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TodoStatus.Pending))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            CreateMap<UpdateTodoRequest, TodoItem>();
        }
    }
}