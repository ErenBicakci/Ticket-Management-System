namespace Aryholding.Tms.AuthService.Application.Commands.Auth.RegisterUser
{
    public class RegisterUserRequestDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
    }
}
