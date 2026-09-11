using Aryholding.Tms.AuthService.Application.Common.Exceptions;
using Aryholding.Tms.AuthService.Application.DTOs;
using Aryholding.Tms.AuthService.Domain.Entities;
using Aryholding.Tms.AuthService.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Aryholding.Tms.AuthService.Application.Commands.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly IUserDepartmentRoleRepository _userDepartmentRoleRepository;

        public LoginUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IOptions<JWT> jwt,
            IUserDepartmentRoleRepository userDepartmentRoleRepository)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _userDepartmentRoleRepository = userDepartmentRoleRepository;
        }

        public async Task<LoginUserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var authenticationModel = new LoginUserResponseDto();
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                authenticationModel.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return authenticationModel;
            }

            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Incorrect Credentials for user {request.Email}.";
            return authenticationModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var userRoles = await _userDepartmentRoleRepository.GetUserRolesWithDetailsAsync(user.Id);
            var roleClaims = new List<Claim>();
            foreach (var userRole in userRoles)
            {
                roleClaims.Add(new Claim("role", userRole.Description ?? ""));
                roleClaims.Add(new Claim("departmentCode", userRole.Department?.DepartmentCode?.ToString() ?? ""));
                roleClaims.Add(new Claim("priority", userRole.PriorityLevel?.Level.ToString() ?? ""));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("username", user.UserName ?? ""),
                new Claim("superUser", user.SuperUser.ToString().ToLower())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
