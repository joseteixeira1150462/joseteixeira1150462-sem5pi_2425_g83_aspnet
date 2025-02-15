using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HealthCare.Domain.Shared
{
    public class JwtTokenService
    {
        private readonly SymmetricSecurityKey Key;
        private readonly JwtSecurityTokenHandler TokenHandler;

        public JwtTokenService(string secretKey)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            TokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(List<Claim> claims, int duration)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "HealthCare",
                audience: "HealthCare",
                claims: claims,
                expires: DateTime.Now.AddHours(duration),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidatePasswordResetToken(string token, Claim typeClaim, Claim emailClaim)
        {
            // Validate the token
            try
            {
                ClaimsPrincipal principal = TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = Key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "HealthCare",
                    ValidAudience = "HealthCare",
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                foreach (var item in principal.Claims)
                {
                    Console.WriteLine($"\n{item.Type}   {item.Value}");
                }

                Console.WriteLine($"\n{typeClaim.Type}   {typeClaim.Value}");
                Console.WriteLine($"\n{emailClaim.Type}   {emailClaim.Value}");

                // Check if the token has the given claim
                return principal.Claims.Any(c => c.Type == typeClaim.Type && c.Value == typeClaim.Value)
                    && principal.Claims.Any(c => c.Type == emailClaim.Type && c.Value == emailClaim.Value);
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
        }

        public bool ValidateEmailUpdateToken(string token, Claim expectedTypeClaim, Claim expectedEmailClaim)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                // Deserializar o token
                var jwtToken = handler.ReadJwtToken(token);

                // Validar tipo de token e email
                var typeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Typ);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "current_email");

                if (typeClaim == null || emailClaim == null)
                {
                    return false; // Faltando claims essenciais
                }

                // Verificar se o tipo e o e-mail do token coincidem com os valores esperados
                if (typeClaim.Value != expectedTypeClaim.Value || emailClaim.Value != expectedEmailClaim.Value)
                {
                    return false; // Token inválido
                }

                return true; // Token válido
            }
            catch (Exception)
            {
                return false; // Token malformado ou inválido
            }
        }

        public string GetClaimFromToken(string token, string claimType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(token))
                    throw new ArgumentException("Invalid token format.");

                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Localizar o claim pelo tipo
                return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve claim '{claimType}' from token.", ex);
            }
        }

    }
}