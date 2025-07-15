namespace OrderApp.Web.Orders.Create;
using AutoMapper=AutoMapper;

public class Create(IOrderEndpointService _endpointService, AutoMapper.IMapper _mapper):Endpoint<CreateOrderRequest, CreateOrderResponse>
{
    public override void Configure()
    {
        Post(CreateOrderRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateOrderRequest request, CancellationToken ct)
    {
        var order = await _endpointService.CreateAsync(request, ct);
        var response = _mapper.Map<CreateOrderResponse>(order);
        await SendAsync(response, cancellation: ct);
    }
}
