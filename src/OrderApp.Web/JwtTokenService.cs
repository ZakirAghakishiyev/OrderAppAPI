using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel.Interfaces;
using Login = OrderApp.Web.Login;

namespace OrderApp.Web;

public class JwtTokenService(IRepository<User> _userRepository, IConfiguration _config)
{
    public async Task<SecurityToken> GenerateTokenAsync(Login.LoginRequest req, string role)
    {
        var keyString = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var expireMinutes = _config["Jwt:ExpireMinutes"];

        if (string.IsNullOrWhiteSpace(keyString))
            throw new InvalidOperationException("JWT Key is missing in configuration.");

        if (!double.TryParse(expireMinutes, out var expireMinutesParsed))
            throw new InvalidOperationException("Invalid JWT expiration value in configuration.");

        var user = await _userRepository.FirstOrDefaultAsync(new UserByNameAndPasswordSpec(req.Username, req.Password));
        if (user == null)
            throw new UnauthorizedAccessException("Invalid username or password.");
        var claims = new[]
        {
            new Claim("id",user.Id.ToString()),
            new Claim("username", req.Username),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(expireMinutesParsed);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return token;
    }
}
