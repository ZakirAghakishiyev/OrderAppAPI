using OrderApp.Core.OrderAggregate;
using OrderApp.Web.Orders.Delete;
using OrderApp.Web.Orders.GetById;
using OrderApp.Web.Orders.List;
using OrderApp.Web.Orders.Update;

namespace OrderApp.Web.Orders.Create;

public interface IOrderEndpointService
{
    public Task<CreateOrderResponse> CreateAsync(CreateOrderRequest req, CancellationToken ct);
    public Task<DeleteOrderResponse?> DeleteAsync(DeleteOrderRequest req, CancellationToken ct);
    public Task<OrderListResponse> ListAsync(CancellationToken ct);
    public Task<GetOrderByIdResponse?> GetByIdAsync(GetOrderByIdRequest req, CancellationToken ct);
    public Task<UpdateOrderResponse?> UpdateAsync(UpdateOrderRequest req, CancellationToken ct);
}