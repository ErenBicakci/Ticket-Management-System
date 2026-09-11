using Aryholding.Tms.GeneralService.API.Attributes;
using Aryholding.Tms.GeneralService.Application.Commands.TicketSeverityManagement.GetTicketSeverities;
using Aryholding.Tms.GeneralService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.GeneralService.API.Controllers
{
    [ApiController]
    [Route("api/severity")]
    public class TicketSeverityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketSeverityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        [CustomRole]
        [RedisCache(expirationMinutes: 10, usernameRequired: false)]
        public async Task<ActionResult<IEnumerable<TicketSeverityResponseDTO>>> GetTicketSeverities()
        {
            var query = new GetTicketSeveritiesCommand();
            var ticketSeverities = await _mediator.Send(query);
            return new OkObjectResult(ticketSeverities);
        }
    }
}
