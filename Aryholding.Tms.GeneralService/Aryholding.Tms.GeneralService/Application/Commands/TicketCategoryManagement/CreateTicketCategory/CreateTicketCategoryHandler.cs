using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.CreateTicketCategory
{
    public class CreateTicketCategoryHandler : IRequestHandler<CreateTicketCategoryCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPriorityLevelRepository _priorityLevelRepository;

        public CreateTicketCategoryHandler(IUserRepository userRepository, ITicketCategoryRepository ticketCategoryRepository, IDepartmentRepository departmentRepository, IPriorityLevelRepository priorityLevelRepository)
        {
            _userRepository = userRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _departmentRepository = departmentRepository;
            _priorityLevelRepository = priorityLevelRepository;
        }

        public async Task<bool> Handle(CreateTicketCategoryCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetByUsernameAsync(request.username);
            var department = await _departmentRepository.GetByCodeAsync(request.Dto.DepartmentCode);

            if (department == null)
            {
                throw new UnauthorizedAccessException("Department not found.");
            }

            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(userId!.Id, 7, department.DepartmentId);
            if (!userHasRole)
            {
                throw new UnauthorizedAccessException("User does not have the required role to create a ticket category.");
            }

            var ticketCategoryExists = await _ticketCategoryRepository.GetByCodeAsync(request.Dto.CategoryCode);
            if (ticketCategoryExists != null)
            {
                throw new Exception($"Ticket category with name '{request.Dto.CategoryName}' already exists.");
            }

            var priorityLevel = await _priorityLevelRepository.GetByLevelAsync(4);
            var ticketCategory = new TicketCategory
            {
                TicketCategoryCode = request.Dto.CategoryCode,
                Name = request.Dto.CategoryName,
                DepartmentId = department.DepartmentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                PriorityLevelId = priorityLevel!.PriorityLevelId
            };

            await _ticketCategoryRepository.AddAsync(ticketCategory);
            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
