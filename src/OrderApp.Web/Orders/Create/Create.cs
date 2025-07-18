namespace OrderApp.Web.Orders.Create;

using AutoMapper = AutoMapper;
using OrderApp.Endpoint.Attributes;
using OrderApp.Core.UserAggregate;

public class Create(IOrderEndpointService _endpointService, AutoMapper.IMapper _mapper):Endpoint<CreateOrderRequest, CreateOrderResponse>
{
    public override void Configure()
    {
        Post(CreateOrderRequest.Route);
        Policies(Endpoint.Constants.Policies.RoutePermissionPolicy);
        Options(opt => opt
            .WithMetadata(new PermissionAttribute(RoleEnum.Admin))
  //          .WithMetadata(new PermissionAttribute(RoleEnum.Customer)) 
        );
    }

    public override async Task HandleAsync(CreateOrderRequest request, CancellationToken ct)
    {
        var order = await _endpointService.CreateAsync(request, ct);
        var response = _mapper.Map<CreateOrderResponse>(order);
        await SendAsync(response, cancellation: ct);
    }
}
