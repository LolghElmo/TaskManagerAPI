using Common.Models;
using IdentityService.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.Features.Register
{
    public class RegisterCommand : IRequest<Result<UserDto>>
    {
        public required RegisterRequestDto RegisterRequest { get; set; }
    }
}
