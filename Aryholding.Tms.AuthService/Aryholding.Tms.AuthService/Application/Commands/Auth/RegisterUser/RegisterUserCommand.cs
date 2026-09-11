using MediatR;

namespace Aryholding.Tms.AuthService.Application.Commands.Auth.RegisterUser
{
    public record RegisterUserCommand(string FirstName, string LastName, string Username, string Email, string Password, string EmployeeId) : IRequest<RegisterUserResponseDto>
    {

    }
}
