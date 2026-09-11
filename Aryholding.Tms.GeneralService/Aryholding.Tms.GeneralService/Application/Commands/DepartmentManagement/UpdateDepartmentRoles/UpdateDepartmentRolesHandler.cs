using Aryholding.Tms.GeneralService.Application.Common.Exceptions;
using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.UpdateDepartmentRoles
{
    public class UpdateDepartmentRolesHandler : IRequestHandler<UpdateDepartmentRolesCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPriorityLevelRepository _priorityLevelRepository;
        private readonly IUserDepartmentRoleRepository _userDepartmentRoleRepository;

        public UpdateDepartmentRolesHandler(IUserRepository userRepository, IDepartmentRepository departmentRepository, IPriorityLevelRepository priorityLevelRepository, IUserDepartmentRoleRepository userDepartmentRoleRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _priorityLevelRepository = priorityLevelRepository;
            _userDepartmentRoleRepository = userDepartmentRoleRepository;
        }

        public async Task<bool> Handle(UpdateDepartmentRolesCommand request, CancellationToken cancellationToken)
        {
            var adminUser = await _userRepository.GetByUsernameAsync(request.AdminUsername)
                ?? throw new NotFoundException("Admin kullanıcı bulunamadı");

            var targetUser = await _userRepository.GetByUsernameAsync(request.Dto.UserUsername)
                ?? throw new NotFoundException("Hedef kullanıcı bulunamadı");

            var department = await _departmentRepository.GetByCodeAsync(request.DepartmentCode)
                ?? throw new NotFoundException("Departman bulunamadı");

            if (request.Dto.UserUsername == request.AdminUsername)
            {
                throw new BusinessException("Kendi rolünüzü değiştiremezsiniz.");
            }

            var adminHasAccess = await _userRepository.UserHasRoleInDepartmentAsync(adminUser.Id, 7, department.DepartmentId);
            if (!adminHasAccess)
            {
                throw new UnauthorizedException("Bu departmanda işlem yapma yetkiniz bulunmuyor.");
            }

            var adminRole = await _userRepository.GetUserDepartmentRoleAsync(adminUser.Id, department.DepartmentId);
            var targetUserRole = await _userRepository.GetUserDepartmentRoleAsync(targetUser.Id, department.DepartmentId);

            if (adminRole?.PriorityLevel == null)
            {
                throw new UnauthorizedException("Bu departmanda yetki seviyeniz bulunamadı.");
            }

            if (targetUserRole != null && adminRole.PriorityLevel.Level <= targetUserRole?.PriorityLevel?.Level)
            {
                throw new BusinessException("Sizden üst veya eşit yetki seviyesindeki kullanıcılara işlem yapamazsınız.");
            }

            if (request.Dto.PriorityLevel == 0)
            {
                if (targetUserRole?.PriorityLevel == null)
                {
                    throw new BusinessException("Kullanıcının bu departmanda zaten yetkisi yok");
                }

                targetUserRole.IsActive = false;
                await _userRepository.SaveChangesAsync();
                return true;
            }

            var requestedPriority = await _priorityLevelRepository.GetByLevelAsync(request.Dto.PriorityLevel)
                ?? throw new NotFoundException("İstenen seviye bulunamadı");

            if (adminRole.PriorityLevel.Level <= requestedPriority.Level)
            {
                throw new BusinessException("Kullanıcıya atanmak istenen rolden daha üst seviyede değilsiniz.");
            }

            if (targetUserRole != null)
            {
                targetUserRole.PriorityLevelId = requestedPriority.PriorityLevelId;
                targetUserRole.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newRole = new UserDepartmentRole
                {
                    UserId = targetUser.Id,
                    DepartmentId = department.DepartmentId,
                    PriorityLevelId = requestedPriority.PriorityLevelId,
                    Description = "Department role assigned by " + adminUser.UserName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _userDepartmentRoleRepository.AddAsync(newRole);
            }

            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
