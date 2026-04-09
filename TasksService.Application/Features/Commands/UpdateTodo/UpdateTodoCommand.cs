using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;

namespace TasksService.Application.Features.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<Result<TodoDto>>
    {
        public required string UserId { get; set; }
        public int Id { get; set; } 
        public required UpdateTodoRequest TodoRequest { get; set; }
    }
}
