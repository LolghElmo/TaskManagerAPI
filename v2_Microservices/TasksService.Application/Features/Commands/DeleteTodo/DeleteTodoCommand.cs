using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TasksService.Application.Features.Commands.DeleteTodo
{
    public class DeleteTodoCommand : IRequest<Result<bool>>
    {
        public required string UserId { get; set; }
        public int Id { get; set; }
    }
}
