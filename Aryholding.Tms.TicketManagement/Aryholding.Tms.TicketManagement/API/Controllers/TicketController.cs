using Aryholding.Tms.GeneralService.Attributes;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.ApproveTicket;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.AssignTicket;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.CreateTicket;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetAssignedTickets;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetDepartmentTickets;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetTickets;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.RejectTicket;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.SubmitTicketForApproval;
using Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.UpdateTicket;
using Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetMyTicketSummary;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.TicketManagement.API.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [CustomRole]
        public async Task<ActionResult<TicketResponseDto>> Create(CreateTicketDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = JwtUtility.GetUsernameFromJwt(User);
            var command = new CreateTicketCommand(createDto, username);
            var ticket = await _mediator.Send(command);
            return Ok(ticket);
        }


        [HttpGet("severity-status-category-user")]
        [CustomRole(minimumPriority: 6)]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetTicketsBySeverityAndStatusAndCategoryAndUser(
            [FromQuery] string? severityCode = null,
            [FromQuery] string? statusCode = null,
            [FromQuery] string? categoryCode = null,
            [FromQuery] string? username = null,
            [FromQuery] int page = 1,
            [FromQuery] string? orderDirection = "desc"
        )
        {
            var query = new GetTicketsCommand(
                severityCode, statusCode, categoryCode, username, page, orderDirection
            );

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }



        [HttpGet("my-tickets")]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetTicketsBySeverityAndStatusAndCategoryAndUser(
            [FromQuery] string? severityCode = null,
            [FromQuery] string? statusCode = null,
            [FromQuery] string? categoryCode = null,
            [FromQuery] int page = 1,
            [FromQuery] string? orderDirection = "desc"
        )
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetTicketsCommand(
                severityCode, statusCode, categoryCode, username, page, orderDirection
            );

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }

        [HttpGet("my-summary")]
        [CustomRole]
        public async Task<ActionResult<UserTicketSummaryDto>> GetMyTicketSummary()
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetMyTicketSummaryQuery(username);
            var summary = await _mediator.Send(query);
            return Ok(summary);
        }


        [HttpGet("assigned-to-me")]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> AssignedToMe(
        [FromQuery] string? severityCode = null,
        [FromQuery] string? statusCode = null,
        [FromQuery] string? categoryCode = null,
        [FromQuery] int page = 1,
        [FromQuery] string? orderDirection = "desc")
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetAssignedTicketsCommand(
                severityCode, statusCode, categoryCode, username, page, orderDirection
            );

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }

        [HttpGet("department-tickets")]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetDepartmentTickets(
        [FromQuery] string? severityCode = null,
        [FromQuery] string? statusCode = null,
        [FromQuery] string? categoryCode = null,
        [FromQuery] string? departmentCode = null,
        [FromQuery] string? assignedUser = null,
        [FromQuery] int page = 1,
        [FromQuery] string? orderDirection = "desc")
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetDepartmentTicketsCommand(
                severityCode, statusCode, categoryCode,departmentCode, username,assignedUser, page, orderDirection
            );

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }


        [HttpPut]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> UpdateTicket(UpdateTicketDto DTO)
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new UpdateTicketCommand(
                DTO, username
            );

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }


        [HttpPut("{ticketId:long}/assign/{assignedUsername}")]
        [CustomRole]
        public async Task<ActionResult<TicketResponseDto>> AssignTicket(long ticketId,string assignedUsername)
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new AssignTicketCommand(
                username, ticketId,assignedUsername
            );

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }

        [HttpPut("{ticketId:long}/submit-for-approval")]
        [CustomRole]
        public async Task<ActionResult<bool>> SubmitForApproval(long ticketId)
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new SubmitTicketForApprovalCommand(
                username, ticketId);

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }

        [HttpPut("{ticketId:long}/approve")]
        [CustomRole(minimumPriority:7)]
        public async Task<ActionResult<bool>> ApproveTicket(long ticketId)
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new ApproveTicketCommand(
                username, ticketId);

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }


        [HttpPut("{ticketId:long}/reject")]
        [CustomRole(minimumPriority: 7)]
        public async Task<ActionResult<bool>> RejectTicket(long ticketId)
        {

            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new RejectTicketCommand(
                username, ticketId);

            var tickets = await _mediator.Send(query);
            return Ok(tickets);
        }


    }
}
