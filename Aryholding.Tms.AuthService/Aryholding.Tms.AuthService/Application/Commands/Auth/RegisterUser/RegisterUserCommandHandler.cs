using Aryholding.Tms.AuthService.Application.Common.Exceptions;
using Aryholding.Tms.AuthService.Domain.Entities;
using Aryholding.Tms.AuthService.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Aryholding.Tms.AuthService.Application.Commands.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserDepartmentRoleRepository _userDepartmentRoleRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public RegisterUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserDepartmentRoleRepository userDepartmentRoleRepository,
            IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _userDepartmentRoleRepository = userDepartmentRoleRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<RegisterUserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var userHasSameEmail = await _userManager.Users
                .IgnoreQueryFilters()
                .Where(u => u.Email == request.Email)
                .FirstOrDefaultAsync();
   
            var userHasSameUsername = await _userManager.Users
                .IgnoreQueryFilters()
                .Where(u => u.UserName == request.Username)
                .FirstOrDefaultAsync();

            if(userHasSameEmail != null || userHasSameUsername != null)
            {
                throw new BusinessException("Username or email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmployeeId = request.EmployeeId,
                IsActive = true,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BusinessException("User could not be created. Please contact your administrator.");
            }

            return new RegisterUserResponseDto
            {
                IsSuccess = true,
                Message = $"User {user.UserName} registered successfully.",
                UserId = user.Id
            };
        }
    }
}
