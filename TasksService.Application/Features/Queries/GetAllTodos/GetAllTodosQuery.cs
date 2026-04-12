using Common.Models;
using MediatR;
using TasksService.Application.DTOs;

namespace TasksService.Application.Features.Queries.GetAllTodos
{
    public class GetAllTodosQuery : IRequest<Result<PagedResult<TodoDto>>>
    {
        public required string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
