using HealthCare.Domain.OperationRequests;
using HealthCare.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationRequestController : ControllerBase
    {
        private readonly OperationRequestService _service;

        public OperationRequestController(OperationRequestService service)
        {
            _service = service;
        }

        // GET: api/OperationRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationRequestDto>> GetById(Guid id)
        {
            var OperationRequest = await _service.GetByIdAsync(new OperationRequestId(id));

            if (OperationRequest == null)
            {
                return NotFound();
            }

            return Ok(OperationRequest);
        }

        // POST: api/OperationRequest
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<OperationRequestDto>> Create(CreateOperationRequestDto dto)
        {
            try {
                var OperationRequest = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = OperationRequest.Id }, OperationRequest);
            }
            catch (BusinessRuleValidationException ex) {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OperationRequestDto>>> Search()
        {
            IQueryCollection query = Request.Query;

            List<OperationRequestDto> result;
            result = await _service.GetAllAOperationRequest();

            if ( result.IsNullOrEmpty() )
                return NotFound();

            return Ok(result);
        }
    }
}