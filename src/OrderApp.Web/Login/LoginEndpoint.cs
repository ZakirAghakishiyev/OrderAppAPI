using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OrderApp.Web.Login;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IConfiguration _config;
    private readonly JwtTokenService _tokenService;

    public LoginEndpoint(IConfiguration config, JwtTokenService tokenService)
    {
        _config = config;
        _tokenService = tokenService;
    }

    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // Dummy check, replace with DB/user service
        if (req.Username != "admin" || req.Password != "1234")
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var token = await _tokenService.GenerateTokenAsync(req, "Admin");

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        await SendAsync(new LoginResponse { Token = jwt });
    }
}
