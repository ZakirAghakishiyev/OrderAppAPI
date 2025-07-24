using OrderApp.Core.UserAggregate;
using OrderApp.Endpoint.Attributes;
using OrderApp.Web.Orders.Create;

namespace OrderApp.Web.Orders.OrderLogs;

public class GetOrderLogs(IOrderEndpointService _orderEndpointService):EndpointWithoutRequest<GetOrderLogsResponse>
{
    public override void Configure()
    {
        Get("/orders/logs");
        Options(opt => opt
            .WithMetadata(new PermissionAttribute(RoleEnum.Admin)));
        Description(x => x.WithName("GetOrderLogs"));
    }

    public override async Task<GetOrderLogsResponse> ExecuteAsync(CancellationToken ct)
    {
        return await _orderEndpointService.GetOrderLogsAsync(ct);
    }
}