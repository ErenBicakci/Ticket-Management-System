
using Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.CreateTicketComment;
using Aryholding.Tms.GeneralService.Attributes;
using Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.GetTicketComments;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.TicketManagement.API.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{ticketId:long}")]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetTicketsBySeverityAndStatusAndCategoryAndUser(
            long ticketId,
            [FromQuery] int page = 1,
            [FromQuery] string? orderDirection = "desc"
        )
        {
            
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetTicketCommentsCommand(ticketId,username,page,orderDirection);

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }

        [HttpPost]
        [CustomRole]
        public async Task<ActionResult<bool>> CreateTicketComment(
            CreateTicketCommentRequestDto dto
        )
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new CreateTicketCommentCommand(dto, username);
            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }

    }
}
