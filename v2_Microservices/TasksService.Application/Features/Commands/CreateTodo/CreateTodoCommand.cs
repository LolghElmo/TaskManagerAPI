using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;

namespace TasksService.Application.Features.Commands.CreateTodo
{
    public class CreateTodoCommand : IRequest<Result<TodoDto>>
    {
        public required string UserId { get; set; }
        public required CreateTodoRequest TodoRequest { get; set; }
    }
}
