using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;
using OrderApp.Web.Orders.Create;

namespace OrderApp.Web.Orders.List;

public class List(IOrderEndpointService _endpointService) : EndpointWithoutRequest<OrderListResponse>
{
    public override void Configure()
    {
        Get("/Orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var orders = await _endpointService.ListAsync(cancellationToken);
        await SendAsync(orders);
    }
}
