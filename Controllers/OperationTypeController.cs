using HealthCare.Domain.OperationTypes;
using HealthCare.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationTypeController : ControllerBase
    {
        private readonly OperationTypeService _service;

        public OperationTypeController(OperationTypeService service)
        {
            _service = service;
        }

        // GET: api/operationType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationTypeDto>> GetById(Guid id)
        {
            var operationType = await _service.GetByIdAsync(new OperationTypeId(id));

            if (operationType == null)
            {
                return NotFound();
            }

            return Ok(operationType);
        }

        // POST: api/operationType
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OperationTypeDto>> Create(CreatingOperationTypeDto dto)
        {
            try
            {
                var operationType = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = operationType.Id }, operationType);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // DELETE: api/operationType/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Deactivate(string id)
        {
            try
            {
                if (await _service.DeactivateAsync(id) == null)
                    return NotFound($"Operation Type with id {id} does not exist");

                return Ok();
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OperationTypeDto>>> Search()
        {
            IQueryCollection query = Request.Query;

            List<OperationTypeDto> result;
            if (!query.IsNullOrEmpty())
                result = await _service.QueryAsync(query);
            else
                result = await _service.GetAllAsync();

            if (result.IsNullOrEmpty())
                return NotFound();

            return Ok(result);
        }
    }
}