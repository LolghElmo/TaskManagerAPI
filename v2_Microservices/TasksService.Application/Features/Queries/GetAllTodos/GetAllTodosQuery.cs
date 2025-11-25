using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;

namespace TasksService.Application.Features.Queries.GetAllTodos
{
    public class GetAllTodosQuery : IRequest<Result<IEnumerable<TodoDto>>>
    {
        public required string UserId { get; set; }
    }
}
