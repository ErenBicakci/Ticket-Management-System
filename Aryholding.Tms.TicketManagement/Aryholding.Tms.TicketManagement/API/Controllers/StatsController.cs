using Aryholding.Tms.GeneralService.Attributes;
using Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetUserTicketStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.TicketManagement.API.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user-ticket-stats")]
        [CustomRole(minimumPriority: 7)]
        public async Task<IActionResult> GetUserTicketStats(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] string? departmentCode = null)
        {
            var fromUtc = from.HasValue ? DateTime.SpecifyKind(from.Value, DateTimeKind.Utc) : (DateTime?)null;
            var toUtc = to.HasValue ? DateTime.SpecifyKind(to.Value, DateTimeKind.Utc) : (DateTime?)null;

            var command = new GetUserTicketStatsCommand(fromUtc, toUtc, departmentCode);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
