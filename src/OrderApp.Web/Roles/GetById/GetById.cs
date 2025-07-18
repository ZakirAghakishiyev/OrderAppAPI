using OrderApp.Core.UserAggregate;
using OrderApp.Endpoint.Attributes;

namespace OrderApp.Web.Roles.GetById;

public class GetById(IRoleEndpointService _roleEndpointService):Endpoint<GetRoleByIdRequest, GetRoleByIdResponse>
{
    public override void Configure()
    {
        Get(GetRoleByIdRequest.Route);
        Options(opt => opt
            .WithMetadata(new PermissionAttribute(RoleEnum.Admin))
        );
    }

    public override async Task<GetRoleByIdResponse> HandleAsync(GetRoleByIdRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleEndpointService.GetByIdAsync(request, cancellationToken);
        if (role == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return null!;
        }

        Response = role;
        return Response;
    }
}
