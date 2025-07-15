using System.Net;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Users.Create;

public class Create(IUserEndpointService _userEndpointService) : Endpoint<CreateUserRequest, CreateUserResponse>
{
    public override void Configure()
    {
        Post(CreateUserRequest.Route);
        AllowAnonymous();
        Validator<CreateUserValidator>();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userEndpointService.CreateAsync(request, cancellationToken);
        var response = user;
        await SendAsync(response, 200, cancellationToken);
    }
}
