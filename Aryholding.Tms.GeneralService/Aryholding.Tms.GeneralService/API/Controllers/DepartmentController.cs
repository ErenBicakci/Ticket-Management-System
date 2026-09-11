using Aryholding.Tms.GeneralService.API.Attributes;
using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.CreateDepartment;
using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentDashboard;
using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartments;
using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.UpdateDepartmentRoles;
using Aryholding.Tms.GeneralService.Application.Commands.UserManagement.GetDepartmentUsersWithDetails;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.GeneralService.API.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        [CustomRole]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDTO>>> GetDepartments()
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetDepartmentsCommand(username);
            var departments = await _mediator.Send(query);
            return new OkObjectResult(departments);
        }

        [HttpGet("dashboard")]
        [CustomRole]
        public async Task<ActionResult<DashboardDto>> GetDepartmentDashboard(
            [FromQuery] string departmentCode)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetDepartmentDashboardCommand(departmentCode, username);
            var departmentDashboard = await _mediator.Send(query);
            return new OkObjectResult(departmentDashboard);
        }

        [HttpPost]
        [CustomRole(departmentCode: "IT", minimumPriority: 7)]
        [ClearCache("departments_all")]
        public async Task<ActionResult<bool>> CreateDepartment(CreateDepartmentDTO dto)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new CreateDepartmentCommand(dto, username);
            var response = await _mediator.Send(query);
            return new OkObjectResult(response);
        }

        [HttpGet("{departmentCode}/users")]
        [CustomRole(minimumPriority: 7)]
        public async Task<ActionResult<UserDTO>> GetDepartmentUsersWithDetails(string departmentCode)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new GetDepartmentUsersWithDetailsCommand(username, departmentCode);
            var users = await _mediator.Send(query);
            return new OkObjectResult(users);
        }

        [HttpPost("{departmentCode}/users/role")]
        [CustomRole(minimumPriority: 7)]
        public async Task<ActionResult<bool>> UpdateDepartmentUserRole(
            string departmentCode, [FromBody] UpdateDepartmentUserRoleDTO dto)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var command = new UpdateDepartmentRolesCommand(dto, departmentCode, username);
            var result = await _mediator.Send(command);
            return new OkObjectResult(result);
        }
    }
}
