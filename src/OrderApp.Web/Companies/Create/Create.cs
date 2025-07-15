using System.Net;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Infrastructure.Data;
using OrderApp.Web.Users;

namespace OrderApp.Web.Companies.Create;

public class Create(ICompanyEndpointService _endpointService) : Endpoint<CreateCompanyRequest, CreateCompanyResponse>
{
    public override void Configure()
    {
        Post(CreateCompanyRequest.Route);
        AllowAnonymous();
        Validator<CreateCompanyValidator>();
    }

    public override async Task HandleAsync(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            await SendAsync(new CreateCompanyResponse(0, "Invalid request"), (int)HttpStatusCode.BadRequest, cancellationToken);
            return;
        }
        var Response = await _endpointService.CreateAsync(request, cancellationToken);
        await SendAsync(Response);
    }

}
