using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Users.Delete;

public class Delete(IUserEndpointService _userEndpointService) : Endpoint<DeleteUserRequest>
{
    public override void Configure()
    {
        Delete(DeleteUserRequest.Route);
        AllowAnonymous();
        Validator<DeleteUserValidator>();
    }

    public override async Task HandleAsync(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userEndpointService.DeleteAsync(request,cancellationToken);
        if (user == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }
        await SendAsync($"User with id {user.Id} deleted");
    }
}
