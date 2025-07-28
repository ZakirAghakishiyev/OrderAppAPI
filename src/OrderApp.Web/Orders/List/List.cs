using Microsoft.EntityFrameworkCore;
using OrderApp.Core.RealTime;
using OrderApp.Infrastructure.Data;
using OrderApp.Web.Orders.Create;

namespace OrderApp.Web.Orders.List;

public class List(IOrderEndpointService _endpointService, INotificationService _notifier) : EndpointWithoutRequest<OrderListResponse>
{
    public override void Configure()
    {
        Get("/Orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var orders = await _endpointService.ListAsync(cancellationToken);
        await _notifier.NotifyOrderCreatedAsync("SignalR Notification: Order List Retrievedq");

        await SendAsync(orders);
    }
}
