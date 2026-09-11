using Aryholding.Tms.GeneralService.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.UserManagement.GetUser
{
    public record GetUserCommand(string adminUsername, string userUsername) : IRequest<UserDTO>
    {
    }
}
