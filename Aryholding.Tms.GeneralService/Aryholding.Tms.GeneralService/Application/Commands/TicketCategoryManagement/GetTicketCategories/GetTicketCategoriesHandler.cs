using Aryholding.Tms.GeneralService.Application.Common.Mappers;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.GetTicketCategories
{
    public class GetTicketCategoriesHandler : IRequestHandler<GetTicketCategoriesCommand, IEnumerable<TicketCategoryResponseDTO>>
    {
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public GetTicketCategoriesHandler(ITicketCategoryRepository ticketCategoryRepository, IDepartmentRepository departmentRepository)
        {
            _ticketCategoryRepository = ticketCategoryRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<TicketCategoryResponseDTO>> Handle(GetTicketCategoriesCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByCodeAsync(request.departmentCode!);
            var ticketCategories = await _ticketCategoryRepository.GetAllByDepartmentAsync(department?.DepartmentId);
            return TicketCategoryMapper.MapToResponseDtos(ticketCategories);
        }
    }
}
