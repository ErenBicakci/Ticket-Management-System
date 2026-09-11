using Aryholding.Tms.GeneralService.Application.Common.Mappers;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartments
{
    public class GetDepartmentsHandler : IRequestHandler<GetDepartmentsCommand, IEnumerable<DepartmentResponseDTO>>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        public GetDepartmentsHandler(IDepartmentRepository departmentRepository, IUserRepository userRepository)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<DepartmentResponseDTO>> Handle(GetDepartmentsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.username);
            var userDepartments = await _userRepository.GetAuthorizedDepartmentsAsync(user!.Id);

            return DepartmentMapper.MapToResponseDtos(userDepartments);
        }
    }
}
