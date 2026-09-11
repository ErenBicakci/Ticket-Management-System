using Aryholding.Tms.GeneralService.Application.Common.Exceptions;
using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.CreateDepartment
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPriorityLevelRepository _priorityLevelRepository;
        private readonly IUserDepartmentRoleRepository _userDepartmentRoleRepository;

        public CreateDepartmentHandler(IUserRepository userRepository, IDepartmentRepository departmentRepository, IPriorityLevelRepository priorityLevelRepository, IUserDepartmentRoleRepository userDepartmentRoleRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _priorityLevelRepository = priorityLevelRepository;
            _userDepartmentRoleRepository = userDepartmentRoleRepository;
        }

        public async Task<bool> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.username);
            var itDepartment = await _departmentRepository.GetByCodeAsync("IT");
            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(user!.Id, 7, itDepartment!.DepartmentId);

            if (!userHasRole)
            {
                throw new UnauthorizedException("User does not have the required role to create a department.");
            }

            var departmentExists = await _departmentRepository.DepartmentIsExitsByCodeAsync(request.dto.DepartmentCode);
            if (departmentExists)
            {
                throw new BusinessException($"Department with code '{request.dto.DepartmentCode}' already exists.");
            }

            var department = new Department
            {
                DepartmentName = request.dto.DepartmentName,
                DepartmentCode = request.dto.DepartmentCode,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _departmentRepository.AddAsync(department);
            await _userRepository.SaveChangesAsync();

            var createdDepartment = await _departmentRepository.GetByCodeAsync(request.dto.DepartmentCode);
            var maxPriorityLevel = await _priorityLevelRepository.GetByLevelAsync(10);
            var newRole = new UserDepartmentRole
            {
                UserId = user.Id,
                DepartmentId = createdDepartment!.DepartmentId,
                PriorityLevelId = maxPriorityLevel!.PriorityLevelId,
                Description = "Department Creator",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _userDepartmentRoleRepository.AddAsync(newRole);
            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
