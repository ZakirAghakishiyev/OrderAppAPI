using Microsoft.AspNetCore.SignalR;
using OrderApp.Core.RealTime;

namespace OrderApp.Infrastructure.RealTime;

public class SignalRNotificationService(IHubContext<OrderHub> hubContext) : INotificationService
{
    public async Task NotifyOrderCreatedAsync(string orderId)
    {
        await hubContext.Clients.All.SendAsync("OrderCreated", orderId);
    }
}
