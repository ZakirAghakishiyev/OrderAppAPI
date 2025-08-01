using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel.Interfaces;
using OrderApp.Web.Login;
using Login = OrderApp.Web.Login;

namespace OrderApp.Web;

public class JwtTokenService(IRepository<User> _userRepository, IOptions<JwtOptions> _options)
{
    public async Task<SecurityToken> GenerateTokenAsync(Login.LoginRequest req, string role)
    {
        var keyString = _options.Value.Key;
        var issuer = _options.Value.Issuer;
        var audience = _options.Value.Audience;
        var expireMinutes = _options.Value.ExpiryMinutes;

        if (string.IsNullOrWhiteSpace(keyString))
            throw new InvalidOperationException("JWT Key is missing in configuration.");

        var user = await _userRepository.FirstOrDefaultAsync(new UserByNameAndPasswordSpec(req.Username, req.Password));
        if (user == null)
            throw new UnauthorizedAccessException("Invalid username or password.");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name, req.Username),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

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
