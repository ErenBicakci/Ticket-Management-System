using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Services.Interfaces;
using Aryholding.Tms.TicketManagement.Domain.Constants;
using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

using Microsoft.EntityFrameworkCore;


namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.CreateTicket
{
    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, TicketResponseDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketEventService _ticketEventService;
        private readonly ITicketEventPublisher _ticketEventPublisher;
        private readonly ITicketStatusRepository _ticketStatusRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ITicketSeverityRepository _ticketSeverityRepository;
        public CreateTicketHandler(ITicketRepository ticketRepository, IUserRepository userRepository,ITicketEventService ticketEventService, ITicketEventPublisher ticketEventPublisher, ITicketStatusRepository ticketStatusRepository, ITicketCategoryRepository ticketCategoryRepository,ITicketSeverityRepository ticketSeverityRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketEventService = ticketEventService;
            _ticketEventPublisher = ticketEventPublisher;
            _ticketStatusRepository = ticketStatusRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _ticketSeverityRepository = ticketSeverityRepository;
        }

        public async Task<TicketResponseDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.Username);
            var ticketStatusId = await _ticketStatusRepository.GetIdByCodeAsync(TicketStatusCodes.Pending);
            var ticketCategory = await _ticketCategoryRepository.GetByCodeAsync(request.Dto.CategoryCode);
            var ticketSeverityId = await _ticketSeverityRepository.GetIdByCodeAsync(request.Dto.SeverityCode);

            if (userId == null)
                throw new NotFoundException("User not found.");

            if (ticketCategory == null)
                throw new NotFoundException("Ticket category not found.");

            if (ticketSeverityId == null)
                throw new NotFoundException("Ticket severity not found.");

            var ticket = new Ticket
            {
                Title = request.Dto.Title,
                Description = request.Dto.Description,
                UserId = userId,
                CategoryId = ticketCategory.Id,
                SeverityId = ticketSeverityId,
                IsActive = true,
                TicketStatusId = ticketStatusId,
                PriorityLevelId = ticketCategory.PriorityLevel?.PriorityLevelId ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Ticket? createdTicket;
            await using (var transaction = await _ticketRepository.BeginTransactionAsync())
            {
                try
                {
                    var result = await _ticketRepository.AddAsync(ticket);
                    await _ticketRepository.SaveChangesAsync();

                    createdTicket = await _ticketRepository.GetByIdAsync(result.Id);
                    if (createdTicket == null)
                        throw new NotFoundException("Created ticket not found.");
                    await _ticketEventService.AddEventAsync(createdTicket.Id, userId, TicketEventTypeCodes.Created, $"Ticket Created {request.Username}");

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            await _ticketEventPublisher.PublishAsync(new TicketEventMessage(
                Guid.NewGuid(),
                TicketEventTypeCodes.Created,
                createdTicket.Id,
                request.Username,
                $"Ticket Created {request.Username}",
                DateTime.UtcNow));

            return TicketMapper.MapToResponseDto(createdTicket);

        }
    }
}
