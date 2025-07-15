namespace OrderApp.Web.Orders.Delete;

using OrderApp.Web.Orders.Create;


public class Delete(IOrderEndpointService _endpointService) : Endpoint<DeleteOrderRequest>
{

    public override void Configure()
    {
        Put(DeleteOrderRequest.Route);
        AllowAnonymous();
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
