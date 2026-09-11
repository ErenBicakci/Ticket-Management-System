using MediatR;

namespace Aryholding.Tms.AuthService.Application.Commands.Auth.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponseDto>
    {

    }
}
