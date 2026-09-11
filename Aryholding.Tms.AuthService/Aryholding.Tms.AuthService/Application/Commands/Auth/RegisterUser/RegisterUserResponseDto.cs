namespace Aryholding.Tms.AuthService.Application.Commands.Auth.RegisterUser
{
    public class RegisterUserResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
    }
}
