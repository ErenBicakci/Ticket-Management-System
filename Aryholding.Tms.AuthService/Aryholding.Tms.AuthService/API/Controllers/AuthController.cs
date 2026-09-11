using Microsoft.AspNetCore.Mvc;
using MediatR;
using Aryholding.Tms.AuthService.Application.Commands.Auth.RegisterUser;
using Aryholding.Tms.AuthService.Application.Commands.Auth.Login;

namespace Aryholding.Tms.AuthService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequestDto requestDto)
        {
            var command = new RegisterUserCommand
            (
                requestDto.FirstName,
                requestDto.LastName,
                requestDto.UserName,
                requestDto.Email,
                requestDto.Password,
                requestDto.EmployeeId
            );

            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }
            
            return Ok(new { message = result.Message, userId = result.UserId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequestDto requestDto)
        {
            var command = new LoginUserCommand(requestDto.Email, requestDto.Password);
            var result = await _mediator.Send(command);
            
            if (!result.IsAuthenticated)
            {
                return Unauthorized(new { message = result.Message });
            }
            
            return Ok(result);
        }

        
        
    }
}
