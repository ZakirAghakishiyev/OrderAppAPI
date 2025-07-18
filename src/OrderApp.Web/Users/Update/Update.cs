
namespace OrderApp.Web.Users.Update;

public class Update(IUserEndpointService _userEndpointService) : Endpoint<UpdateUserRequest, UpdateUserResponse>
{

    public override void Configure()
    {
        Put(UpdateUserRequest.Route);
        AllowAnonymous();
        Validator<UpdateUserValidator>();
    }

    public override async Task HandleAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userEndpointService.UpdateAsync(request,cancellationToken);
        if (user == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }
        await SendOkAsync(user, cancellationToken);
    }
}
