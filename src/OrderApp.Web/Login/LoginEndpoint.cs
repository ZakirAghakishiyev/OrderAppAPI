using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Web.Login;

public class LoginEndpoint(IRepository<User> _userRepository, JwtTokenService _tokenService) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // Dummy check, replace with DB/user service
        if (await _userRepository.FirstOrDefaultAsync(new UserByNameAndPasswordSpec(req.Username, req.Password), ct) is not User user)

        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var token = await _tokenService.GenerateTokenAsync(req, "Admin");

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        await SendAsync(new LoginResponse { Token = jwt });
    }
}
