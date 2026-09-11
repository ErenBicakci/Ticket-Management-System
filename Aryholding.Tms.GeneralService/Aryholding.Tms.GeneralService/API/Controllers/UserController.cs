using Aryholding.Tms.GeneralService.API.Attributes;
using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentUsers;
using Aryholding.Tms.GeneralService.Application.Commands.UserManagement.GetUser;
using Aryholding.Tms.GeneralService.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.GeneralService.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("department-users")]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<DepartmentUserDTO>>> GetDepartmentUsers(
            [FromQuery] string departmentCode)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetDepartmentUsersCommand(departmentCode, username);
            var users = await _mediator.Send(query);
            return new OkObjectResult(users);
        }

        [HttpGet("{username}")]
        [CustomRole(minimumPriority: 7)]
        public async Task<ActionResult<IEnumerable<DepartmentUserDTO>>> GetUser(string username)
        {
            var jwtUser = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetUserCommand(jwtUser, username);
            var user = await _mediator.Send(query);
            return new OkObjectResult(user);
        }
    }
}
