using Aryholding.Tms.TicketManagement.Application.Commands.TicketHistoryManagement.GetTicketHistory;
using Aryholding.Tms.GeneralService.Attributes;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.ApproveTicket;
using Aryholding.Tms.TicketManagement.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.TicketManagement.API.Controllers
{

    [ApiController]
    [Route("api/ticketHistory")]
    public class TicketHistoryController : ControllerBase
    {


        private readonly IMediator _mediator;

        public TicketHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{ticketId:long}")]
        [CustomRole]
        public async Task<ActionResult<bool>> GetTicketHistories(long ticketId)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetTicketHistoryCommand(
                ticketId, username);
            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }
    }
}
