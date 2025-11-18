using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Application.Models.DTOs.Task;
using TaskManagerAPI.Core.Interfaces;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Application.Features.Tasks
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTaskCommandHandler> _logger;
        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper, ILogger<CreateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new Exception("User ID cannot be null.");
            }
            // Map the command to TaskItem entity
            var task = _mapper.Map<TaskItem>(request);
            task.CreatedDate = DateTime.UtcNow;
            task.IsCompleted = false;

            // Create the task in the repository
            var createdTask = await _taskRepository.CreateTaskAsync(task);
            _logger.LogInformation("Task created with ID: {TaskId} by User ID: {UserId}", createdTask.Id, request.UserId);

            // Map the created task to TaskDto and return
            return _mapper.Map<TaskDto>(createdTask);
        }
    }
}
