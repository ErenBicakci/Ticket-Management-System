using Aryholding.Tms.GeneralService.Application.Common.Exceptions;
using Aryholding.Tms.GeneralService.Application.Common.Mappers;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.UserManagement.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDepartmentRoleRepository _userDepartmentRoleRepository;

        public GetUserHandler(IUserRepository userRepository, IUserDepartmentRoleRepository userDepartmentRoleRepository)
        {
            _userRepository = userRepository;
            _userDepartmentRoleRepository = userDepartmentRoleRepository;
        }

        public async Task<UserDTO> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.userUsername);
            if (user == null)
            {
                throw new BusinessException("User not found.");
            }
            return UserMapper.MapToResponseDto(user);
        }
    }
}
