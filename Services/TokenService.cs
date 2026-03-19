using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsuariosAPI.Models;

namespace UsuariosAPI.Services
{
    public interface ITokenService
    {
        (string token, DateTime expiration) GenerateAccessToken(CuentaUsuario cuenta);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public (string token, DateTime expiration) GenerateAccessToken(CuentaUsuario cuenta)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey   = jwtSettings["SecretKey"]!;
            var issuer      = jwtSettings["Issuer"]   ?? "UsuariosAPI";
            var audience    = jwtSettings["Audience"] ?? "UsuariosAPIClients";
            var expiryMin   = int.Parse(jwtSettings["AccessTokenExpiryMinutes"] ?? "60");

            var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds      = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(expiryMin);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,  cuenta.Username),
                new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,               cuenta.Username),
                new Claim("userId",    cuenta.UsuarioId.ToString()),
                new Claim("accountId", cuenta.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                notBefore:          DateTime.UtcNow,
                expires:            expiration,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey   = jwtSettings["SecretKey"]!;

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = jwtSettings["Issuer"]   ?? "UsuariosAPI",
                ValidAudience            = jwtSettings["Audience"] ?? "UsuariosAPIClients",
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var handler   = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, validationParams, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido.");
            }

            return principal;
        }
    }
}
