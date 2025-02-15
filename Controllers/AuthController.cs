using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HealthCare.Domain.Shared.Authentication;
using System.Security.Authentication;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _service;

        public AuthController(AuthenticationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<LoggedInDto>> Login(LoginDto dto)
        {
            try
            {
                LoggedInDto authDto = await _service.Login(dto);

                return Ok(authDto);
            }
            catch(InvalidCredentialException ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
    }
}