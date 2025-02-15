using Microsoft.AspNetCore.Mvc;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Users;
using HealthCare.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service)
        {
            _service = service;
        }

        // GET: api/patients/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetById(Guid id)
        {
            var patient = await _service.GetPatientByIdAsync(new PatientId(id));

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        //GET: api/patients/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Guid>> GetIdByUserIdAsync(Guid userId)
        {
            var patientId = await _service.GetPatientIdByUserId(new UserId(userId));

            if (patientId == Guid.Empty)
            {
                return NotFound();
            }

            return Ok(patientId);
        }

        // POST: api/patients
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> Create(CreatingPatientDto dto)
        {
            try
            {
                PatientDto patient = await _service.AddPatientAsync(dto);

                return Ok(patient);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<PatientDto>> Update(string email, [FromBody] EditingPatientDto dto)
        {
            try
            {
                // Verifica se o email na URL corresponde ao email no corpo (dto)
                if (email != dto.Email)
                {
                    return BadRequest(new { Message = "Email in the body does not match the email in the URL." });
                }

                // Chama o serviço para atualizar os dados do paciente (não pode alterar o e-mail nesta fase)
                PatientDto updatedPatient = await _service.UpdatePatientAsync(dto);

                // Retorna o paciente atualizado com status OK
                return Ok(updatedPatient);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        //List All Patient Profiles
        [HttpGet]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ListPatientDto>>> GetAll()
        {
            var patients = await _service.GetAllPatientsAsync();

            if(patients == null){
                return BadRequest();
            }
            
            return Ok(patients);
        }
    }
}