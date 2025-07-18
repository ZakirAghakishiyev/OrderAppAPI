namespace OrderApp.Web.RegisterEndpoint;

public class Resgister(RegisterEndpointService _endpointService) : Endpoint<RegisterRequest>
{
    public override void Configure()
    {
        Post("auth/Register");
        AllowAnonymous();
        Description(x => x
            .WithName("Register")
            .Produces<RegisterResponse>()
            .ProducesProblem(400)
            .ProducesProblem(500));
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        try
        {
            var user = await _endpointService.RegisterAsync(req, ct);
            if (user==null||user.Username == null)
            {
                throw new InvalidOperationException(user!.Email??"Registration failed.");
            }
            await SendAsync(user, cancellation: ct);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception");
            await SendAsync(new { Error = ex.Message }, 400, ct);
        }
    }
}
