namespace OrderApp.Web.Orders.Delete;

using OrderApp.Core.UserAggregate;
using OrderApp.Endpoint.Attributes;
using OrderApp.Web.Orders.Create;


public class Delete(IOrderEndpointService _endpointService) : Endpoint<DeleteOrderRequest>
{

    public override void Configure()
    {
        Delete(DeleteOrderRequest.Route);
        Policies(Endpoint.Constants.Policies.RoutePermissionPolicy);
        Options(opt => opt
            .WithMetadata(new PermissionAttribute(RoleEnum.Admin))
//            .WithMetadata(new PermissionAttribute(RoleEnum.Customer)) 
        );
    }

    public override async Task HandleAsync(DeleteOrderRequest request, CancellationToken ct)
    {
        var deletedOrder=await _endpointService.DeleteAsync(request, ct);
        System.Console.WriteLine(deletedOrder==null?"null":"not null");
        if (deletedOrder == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendAsync($"Order with id {deletedOrder.Id} deleted");
    }

}
