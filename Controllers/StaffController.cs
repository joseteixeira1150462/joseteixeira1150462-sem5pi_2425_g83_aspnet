using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _service;

        public StaffController(StaffService service)
        {
            _service = service;
        }

        // GET: api/staff
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
        {
            var staffList = await _service.GetAllAStaff();

            return Ok(staffList);
        }

        // GET: api/staff/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetById(Guid id)
        {
            var staff = await _service.GetByIdAsync(new StaffId(id));

            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

        // POST: api/staff
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StaffDto>> Create(CreatingStaffDto dto)
        {
            try
            {
                var staff = await _service.AddAsync(dto);

                // Retorna o Staff recém-criado com o status 201 Created e a URL do recurso criado
                return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{licenseNumber}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StaffDto>> Update(string licenseNumber, [FromBody] EditStaffDto dto)
        {
            // Usando o método de fábrica para gerar uma instância de StaffLicenseNumber
            var staffLicenseNumber = StaffLicenseNumber.CreateFromExistingString(licenseNumber);

            try
            {
                // Chama o serviço para atualizar o Staff
                var updatedStaff = await _service.UpdateAsync(staffLicenseNumber, dto);

                if (updatedStaff == null)
                {
                    return NotFound();
                }

                return Ok(updatedStaff);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpDelete("{licenseNumber}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StaffDto>> DeleteStaff(string licenseNumber)
        {
            try
            {
                var staffDto = await _service.DeleteStaffAsync(licenseNumber);

                if (staffDto == null)
                {
                    return NotFound();
                }

                return Ok(staffDto);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }


        }
    }
}