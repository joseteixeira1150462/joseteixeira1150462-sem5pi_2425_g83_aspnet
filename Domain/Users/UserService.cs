using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using HealthCare.Domain.Patients;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Shared.Mailing;
using HealthCare.Domain.Staffs;
using HealthCare.Logs;
using Microsoft.Extensions.Configuration;

namespace HealthCare.Domain.Users
{
    public class UserService
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repository;
        private readonly IStaffRepository _staffRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly EmailService _emailService;
        private readonly JwtTokenService _tokenService;
        private readonly LogChangesService _logChangesService;

        private readonly IConfiguration _configuration;

        public UserService(
            IUnitOfWork unitOfWork,
            IUserRepository repository,
            IStaffRepository staffRepository,
            IPatientRepository patientRepository,
            EmailService emailService,
            JwtTokenService tokenService,
            IConfiguration configuration
        )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _staffRepository = staffRepository;
            _patientRepository = patientRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _logChangesService = new LogChangesService();
            _configuration = configuration;
        }

        public async Task<UserDto> GetByIdAsync(UserId id)
        {
            User user = await this._repository.GetByIdAsync(id);

            if (user == null)
                return null;

            return UserMapper.ToDto(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            User user = await this._repository.GetByEmail(email);

            if (user == null)
                return null;

            return UserMapper.ToDto(user);
        }

        public async Task<UserDto> AddStaffUserAsync(CreatingUserDto dto)
        {
            Staff staffProfile = await _staffRepository.GetByEmailAsync(dto.Email);

            if (staffProfile == null)
                return null;

            User user = new User(dto.Email, dto.Role);
            user.RefreshSession();
            user.ResetLoginAttempts();

            await _repository.AddAsync(user);

            staffProfile.AssociateUser(user);

            await _unitOfWork.CommitAsync();

            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Typ, "password_reset"),
                new Claim("user_email", user.Email.Address),
            };
            string token = _tokenService.GenerateToken(claims, 24);

            string link = $"http://localhost:80/users/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email.Address)}";

            string body = EmailTemplates.AccountSetup(user.StaffProfile.FirstName, link);
            await _emailService.SendEmailAsync(_configuration["EmailSettings:TestingAddress"], "Setup account", body);

            return UserMapper.ToDto(user);
        }

        public async Task<UserDto> AddPatientUserAsync(CreatingPatientUserDto dto)
        {

            Patient patientProfile = await _patientRepository.GetByEmailAsync(dto.Email);

            if (patientProfile == null)
                return null;

            User user = new User(dto.Email, dto.Role);
            user.SetPassword(dto.Password);
            user.RefreshSession();
            user.ResetLoginAttempts();

            await _repository.AddAsync(user);

            patientProfile.AssociateUser(user);

            await _unitOfWork.CommitAsync();

            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Typ, "account_activation"),
                new Claim("user_email", user.Email.Address),
            };
            string token = _tokenService.GenerateToken(claims, 24);

            string link = $"http://localhost:80/users/activation?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email.Address)}";

            string body = EmailTemplates.AccountSetup(user.PatientProfile.FirstName, link);
            await _emailService.SendEmailAsync(_configuration["EmailSettings:TestingAddress"], "Confirm email", body);

            return UserMapper.ToDto(user);
        }

        public async Task<UserDto> SetPasswordAndActivateAsync(UpdateUserPasswordDto dto, string token)
        {
            Claim typeClaim = new Claim(JwtRegisteredClaimNames.Typ, "password_reset");
            Claim emailClaim = new Claim("user_email", dto.Email);

            if (!_tokenService.ValidatePasswordResetToken(token, typeClaim, emailClaim))
                throw new ValidationException("Invalid bearer token");

            User user = await _repository.GetByEmail(dto.Email);

            if (user == null)
                return null;

            user.SetPassword(dto.Password);
            user.MarkAsActive();

            await _unitOfWork.CommitAsync();

            string body = EmailTemplates.AccountSetupConfirmation();
            await _emailService.SendEmailAsync(_configuration["EmailSettings:TestingAddress"], "Account Activated", body);

            return UserMapper.ToDto(user);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            User user = await _repository.GetByEmail(email);

            if (user == null)
                return false;

            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Typ, "password_reset"),
                new Claim("user_email", user.Email.Address)
            };
            string token = _tokenService.GenerateToken(claims, 24);

            string link = $"http://localhost:80/users/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email.Address)}";

            string body = EmailTemplates.UserPasswordReset(user.StaffProfile.FirstName, link);
            await _emailService.SendEmailAsync(_configuration["EmailSettings:TestingAddress"], "Password Reset", body);

            return true;
        }

        public async Task<UserDto> ResetPasswordAsync(UpdateUserPasswordDto dto, string token)
        {
            Claim typeClaim = new Claim(JwtRegisteredClaimNames.Typ, "password_reset");
            Claim emailClaim = new Claim("user_email", dto.Email);

            if (!_tokenService.ValidatePasswordResetToken(token, typeClaim, emailClaim))
                throw new ValidationException("Invalid bearer token");

            User user = await _repository.GetByEmail(dto.Email);

            if (user == null)
                return null;

            if (!user.Active)
                throw new ResourceUnavailableException("This user is deactivated");

            user.SetPassword(dto.Password);

            await _unitOfWork.CommitAsync();

            return UserMapper.ToDto(user);
        }

        public async Task<bool> VerifyEmailAsync(string token, string email)
        {
            Claim typeClaim = new Claim(JwtRegisteredClaimNames.Typ, "account_activation");
            Claim emailClaim = new Claim("user_email", email);

            if (!_tokenService.ValidatePasswordResetToken(token, typeClaim, emailClaim))
                throw new ValidationException("Invalid bearer token");

            User user = await _repository.GetByEmail(email);

            if (user == null)
                return false;

            user.MarkAsActive();
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<User> RequestEmailUpdateAsync(UpdateUserEmailDto dto)
        {
            // Buscar o usuário pelo email atual
            var user = await _repository.GetByEmail(dto.CurrEmail.Address);
            if (user == null)
                throw new ValidationException("No user found with the provided email.");

            var patient = await _patientRepository.GetByEmailAsync(user.Email.Address);
            if (patient == null)
                throw new ValidationException("No patient found with the provided email.");

            if (await _repository.GetByEmail(dto.NewEmail.Address) != null)
                throw new ValidationException("The new email is already in use.");

            // Gerar o token JWT para a operação de atualização de email
            string token = _tokenService.GenerateToken(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Typ, "email_update"),
                new Claim("current_email", dto.CurrEmail.Address),
                new Claim("new_email", dto.NewEmail.Address)
            }, duration: 24); // Token válido por 24 horas

            // Montar o link de confirmação
            string confirmationLink = $"http://localhost:5000/api/users/edit-email/{dto.CurrEmail.Address}?token={token}";

            // Criar corpo do email
            string body = EmailTemplates.UserEmailUpdate(user.PatientProfile.FirstName, confirmationLink);

            // Enviar email com o link de confirmação
            await _emailService.SendEmailAsync(_configuration["EmailSettings:TestingAddress"], "Email Update Confirmation", body);

            return user;
        }

        public async Task<bool> UpdateEmailAsync(string currentEmail, string token)
        {
            // Validar token e e-mail atual
            var typeClaim = new Claim(JwtRegisteredClaimNames.Typ, "email_update");
            var emailClaim = new Claim("current_email", currentEmail);

            if (!_tokenService.ValidateEmailUpdateToken(token, typeClaim, emailClaim))
            {
                throw new ValidationException("Invalid or expired token.");
            }

            // Buscar usuário pelo e-mail atual
            var user = await _repository.GetByEmail(currentEmail);
            if (user == null)
            {
                throw new ValidationException("User not found.");
            }

            // Obter novo e-mail a partir do token
            var newEmail = _tokenService.GetClaimFromToken(token, "new_email");
            if (string.IsNullOrEmpty(newEmail))
            {
                throw new ValidationException("New email not found in token.");
            }

            // Validar se o novo e-mail já está em uso
            var existingUser = await _repository.GetByEmail(newEmail);
            if (existingUser != null)
            {
                throw new ValidationException("Email already in use.");
            }

            // Atualizar o e-mail do usuário e desativar a conta temporariamente
            user.Email = new UserEmail(newEmail);
            //user.MarkAsInactive(); -> this is unnecessary because the user confirms the email update.//only use if soft delete is performed.

            // Atualizar o perfil do paciente, se aplicável
            var patientProfile = await _patientRepository.GetByEmailAsync(currentEmail);
            if (patientProfile != null)
            {
                patientProfile.Email = new PatientEmail(newEmail);
                
            }

            var oldPatientDto = new PatientDto
            {
                Id = patientProfile.Id.AsGuid(),
                FirstName = patientProfile.FirstName,
                LastName = patientProfile.LastName,
                FullName = patientProfile.FullName,
                BirthDate = patientProfile.BirthDate,
                Gender = patientProfile.Gender,
                MedicalRecordNumber = patientProfile.MedicalRecordNumber.Number,
                Email = currentEmail,
                Phone = patientProfile.Phone.Value,
                HealthConditions = patientProfile.HealthConditions,
                EmergencyPhone = patientProfile.EmergencyPhone.Value,
                SequentialNumber = patientProfile.SequentialNumber,
                UpdatesExecuted = patientProfile.UpdatesExecuted
            };

            patientProfile.IncrementUpdateAttempts();
            await _patientRepository.UpdateAsync(patientProfile);

            await _repository.UpdateAsync(user);


            // Registra o log de atualização de e-mail
            await _unitOfWork.CommitAsync();
            await _logChangesService.AppendEmailUpdateLogAsync(oldPatientDto, newEmail, patientProfile.Id); //falta try catch?
            return true;
        }
    }
}