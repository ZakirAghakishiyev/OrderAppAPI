using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;
using OrderApp.Web.Orders.Create;

namespace OrderApp.Web.Orders.Update;

public class Update(IOrderEndpointService _endpointService) : Endpoint<UpdateOrderRequest, UpdateOrderResponse>
{
    public override void Configure()
    {
        Put(UpdateOrderRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateOrderRequest request, CancellationToken ct)
    {
        var order = await _endpointService.UpdateAsync(request,ct);

        if (order == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendAsync(order);
    }
}
