using Aryholding.Tms.GeneralService.API.Attributes;
using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentUsers;
using Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.CreateTicketCategory;
using Aryholding.Tms.GeneralService.Application.Commands.TicketCategoryManagement.GetTicketCategories;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aryholding.Tms.GeneralService.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class TicketCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        [CustomRole]
        [RedisCache(expirationMinutes: 10, cacheKey: "categories_all", usernameRequired: false)]
        public async Task<ActionResult<IEnumerable<TicketCategoryResponseDTO>>> GetCategories(
            [FromQuery] string? departmentCode)
        {
            var query = new GetTicketCategoriesCommand(departmentCode);
            var categories = await _mediator.Send(query);
            return new OkObjectResult(categories);
        }

        [HttpPost]
        [CustomRole]
        [ClearCache("categories_all")]
        public async Task<ActionResult<bool>> CreateTicketCategory(CreateTicketCategoryRequestDto dto)
        {
            var username = JwtUtility.GetUsernameFromJwt(User);
            var query = new CreateTicketCategoryCommand(dto, username);
            var response = await _mediator.Send(query);
            return new OkObjectResult(response);
        }
    }
}
