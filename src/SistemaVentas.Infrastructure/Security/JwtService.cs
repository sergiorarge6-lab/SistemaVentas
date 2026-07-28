using Microsoft.Extensions.Configuration;
using SistemaVentas.Application.Interfaces.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaVentas.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarToken(
        int id,
        string usuario,
        string rol)
    {
        string key = _configuration["Jwt:Key"]!;

        string issuer = _configuration["Jwt:Issuer"]!;

        string audience = _configuration["Jwt:Audience"]!;

        int expireMinutes =
            int.Parse(_configuration["Jwt:ExpireMinutes"]!);

        // la key a UTF8
        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        SigningCredentials credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario),
            new Claim(ClaimTypes.Role, rol)
        ];

        JwtSecurityToken token =
            new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}