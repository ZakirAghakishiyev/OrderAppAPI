using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;
using OrderApp.Web.Orders.Create;
using Mapper = AutoMapper;

namespace OrderApp.Web.Orders.GetById;

public class GetById(IOrderEndpointService _endpointService, Mapper.IMapper _mapper) : Endpoint<GetOrderByIdRequest>
{
    public override void Configure()
    {
        Get(GetOrderByIdRequest.Route);
        AllowAnonymous();
        Validator<GetOrderByIdValidator>();
    }

    public override async Task HandleAsync(GetOrderByIdRequest request, CancellationToken ct)
    {
        var order = await _endpointService.GetByIdAsync(request,ct);
        if (order == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = _mapper.Map<OrderRecord>(order);
        await SendOkAsync(Response, ct);
    } 

}
