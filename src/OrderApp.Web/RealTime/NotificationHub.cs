using Microsoft.AspNetCore.SignalR;

namespace OrderApp.Web.RealTime;

public class NotificationHub : Hub
{
    public async Task SendOrderNotification(string orderId)
    {
        await Clients.All.SendAsync("ReceiveNotification", $"New order received: {orderId}");
    }
}
