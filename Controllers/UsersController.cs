using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Users;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using HealthCare.Domain.Patients;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            UserDto user = await _service.GetByIdAsync(new UserId(id));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("staff")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateStaffUser(CreatingUserDto dto)
        {
            try
            {
                UserDto user = await _service.AddStaffUserAsync(dto);

                if (user == null)
                {
                    return NotFound(new { message = "No Staff profile found with provided e-mail" });
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpPost("patient")]
        public async Task<ActionResult<UserDto>> CreatePatientUser(CreatingPatientUserDto dto)
        {
            try
            {
                UserDto user = await _service.AddPatientUserAsync(dto);

                if (user == null)
                {
                    return NotFound(new { message = "No Patient profile found with provided e-mail" });
                }

                return Ok(user);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpPatch("email/{email}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<UserDto>> RequestEmailUpdate(string email, [FromBody] UpdateUserEmailDto dto)
        {
            try
            {
                if (email != dto.CurrEmail.Address || dto == null || dto.CurrEmail == null || dto.NewEmail == null)
                {
                    return BadRequest("Invalid request. Both current and new email must be provided.");
                }
                await _service.RequestEmailUpdateAsync(dto);
                return Ok(new { message = "A confirmation email has been sent to the new email address." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation error occurred while requesting email update.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred.",
                    details = ex.Message
                });
            }

        }


        [HttpPatch("setup")]
        public async Task<ActionResult<UserDto>> SetPasswordAndActivate(UpdateUserPasswordDto dto)
        {
            string authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Missing or invalid Authorization header.");

            string token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                UserDto user = await _service.SetPasswordAndActivateAsync(dto, token);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("reset-password/{email}")]
        [Authorize(Roles = "Admin,Doctor,Nurse,Technician")]
        public async Task<IActionResult> SendPasswordResetEmail(string email)
        {
            try
            {
                bool successful = await _service.SendPasswordResetEmailAsync(email);

                if (!successful)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPatch("reset-password")]
        [Authorize(Roles = "Admin,Doctor,Nurse,Technician")]
        public async Task<ActionResult<UserDto>> ResetPassword(UpdateUserPasswordDto dto)
        {
            string authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Missing or invalid Authorization header.");

            string token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                UserDto user = await _service.ResetPasswordAsync(dto, token);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (ResourceUnavailableException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // GET: api/users/verify-email
        [HttpGet("verify-email/{email}")]
        public async Task<IActionResult> VerifyEmailAsync(string email)
        {
            // Extrair o token do cabeçalho Authorization
            var bearerToken = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(bearerToken) || !bearerToken.StartsWith("Bearer "))
            {
                return BadRequest("Bearer token is missing or invalid");
            }

            // Remover o prefixo "Bearer " do token
            var token = bearerToken.Substring("Bearer ".Length).Trim();

            // Verificar o e-mail e o token
            bool isVerified = await _service.VerifyEmailAsync(token, email);

            if (!isVerified)
            {
                return BadRequest("Invalid token or email");
            }

            return Ok("Email successfully verified");
        }

        [HttpGet("verify-update-email/{email}")]
        public async Task<IActionResult> VerifyUpdateEmailAsync(string email)
        {
            try
            {
                // Obter o token do cabeçalho Authorization
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Authorization token is missing or invalid.");
                }

                var token = authorizationHeader.Replace("Bearer ", "");

                // Chamar o serviço para verificar e atualizar o e-mail
                bool isUpdated = await _service.UpdateEmailAsync(email, token);

                if (!isUpdated)
                {
                    return BadRequest("Email update failed. Please ensure the email is correct or not already in use.");
                }

                return Ok("Email successfully updated.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}