namespace Aryholding.Tms.AuthService.Application.Commands.Auth.Login
{
    public class LoginUserResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
