using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Abstra.Challenge.Presentation.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    [HttpPost("token")]
    [ProducesResponseType<AuthenticationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] AuthenticationRequest request)
    {
        if (request.Username != "admin" || request.Password != "admin")
            return Unauthorized();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConstants.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims =
            new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        
        var token =
            new JwtSecurityToken(
                issuer: AuthenticationConstants.JwtIssuer,
                audience: AuthenticationConstants.JwtAudience,
                claims: claims,
                expires: TimeProvider.System.GetUtcNow().DateTime.AddHours(1),
                signingCredentials: credentials);
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return Ok(new AuthenticationResponse(tokenString));
    }
}
