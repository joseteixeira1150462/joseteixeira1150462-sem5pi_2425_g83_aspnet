using System.Security.Authentication;
using System.Security.Claims;
using HealthCare.Domain.Users;

namespace HealthCare.Domain.Shared.Authentication
{
    public class AuthenticationService
    {
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        private JwtTokenService _tokenService;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            JwtTokenService tokenService
        )
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<LoggedInDto> Login(LoginDto dto)
        {
            User user = await _userRepository.GetByEmail(dto.Email);

            if (user == null)
                throw new InvalidCredentialException("No account with provided username");

            if (user.LoginAttempts == 0)
            {
                throw new InvalidCredentialException("Account locked, password reset needed");
            }

            string dtoPasswordHash = UserPassword.CreateFromPlainText(dto.Password).Hash;

            if (user.PasswordHash.Hash != dtoPasswordHash)
            {
                user.DecrementLoginAttempts();

                await _unitOfWork.CommitAsync();

                throw new InvalidCredentialException("Password doesn't match");
            }

            user.RefreshSession();
            user.ResetLoginAttempts();

            await _unitOfWork.CommitAsync();

            var claims = new List<Claim>{
                new Claim("type", "auth"),
                new Claim("uid", user.Id.AsString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            string token = _tokenService.GenerateToken(claims, 1);

            return new LoggedInDto(token, user.Id.AsString());
        }

        public async Task<bool> UserTimedOut(string uid, TimeSpan timeout)
        {
            User user = await _userRepository.GetByIdAsync(new UserId(uid));

            if (user == null)
                throw new InvalidCredentialException("Invalid token");

            if ((DateTime.Now - user.LastRequest) > timeout)
            {
                return true;
            }

            user.RefreshSession();

            await _unitOfWork.CommitAsync();

            return false;
        }
    }
}
