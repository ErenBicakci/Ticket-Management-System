namespace Aryholding.Tms.AuthService.Application.Commands.Auth.Login
{
    public class LoginUserRequestDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
