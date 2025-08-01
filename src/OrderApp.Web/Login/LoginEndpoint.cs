using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Web.Login;

public class LoginEndpoint(IRepository<User> _userRepository, JwtTokenService _tokenService, IDistributedCache _cache) : Endpoint<LoginRequest, LoginResponse>
{
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(300);

    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        string tokenId = Guid.NewGuid().ToString();
        string cacheKey = $"jwt:{tokenId}";

        if (await _userRepository.FirstOrDefaultAsync(new UserByNameAndPasswordSpec(req.Username, req.Password), ct) is not User user)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var token = await _tokenService.GenerateTokenAsync(req, "Admin");
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        var userData = new
        {
            Token = jwt,
            UserId = user.Id,
            Username = user.Name,
            Expiry = DateTime.UtcNow.Add(_cacheExpiry)
        };
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiry
        };
        byte[] data = JsonSerializer.SerializeToUtf8Bytes(userData);
        var userDataString = userData.ToString();
        await _cache.SetStringAsync(cacheKey, userDataString!, options, ct);
        Console.WriteLine("Cached: "+await _cache.GetStringAsync(cacheKey, ct));
        await SendAsync(new LoginResponse { Token = jwt });
    }
}
