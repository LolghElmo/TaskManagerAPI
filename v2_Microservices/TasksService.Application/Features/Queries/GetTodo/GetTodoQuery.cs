using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;

namespace TasksService.Application.Features.Queries.GetTodo
{
    public class GetTodoQuery : IRequest<Result<TodoDto>>
    {
        public required string UserId { get; set; }
        public int Id { get; set; }
    }
}
