using Microsoft.AspNetCore.Http.HttpResults;
using MO = OrderApp.Core.OrderAggregate;
using OrderApp.Web.Orders.Delete;
using OrderApp.Web.Orders.GetById;
using OrderApp.Web.Orders.Update;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderApp.Core.OrderAggregate.Specification;
using OrderApp.Web.Orders.List;
using AutoMap = AutoMapper;
using Ardalis.Specification;
using SK = OrderApp.SharedKernel.Interfaces;
using System.Linq;
using OrderApp.Web.Orders.OrderLogs;
using OrderApp.Core.OrderAggregate;
using OrderApp.Core.OrderAggregate.Specifications;

namespace OrderApp.Web.Orders.Create;

public class OrderEndpointService(SK.IRepository<MO.Order> _orderRepository, SK.IRepository<MO.LoggedOrder> _logRepository, AutoMap.IMapper _mapper) : IOrderEndpointService
{
    public async Task<CreateOrderResponse> CreateAsync(CreateOrderRequest req, CancellationToken ct)
    {
        try
        {
            var order = _mapper.Map<MO.Order>(req);


            await _orderRepository.AddAsync(order, ct);
            return _mapper.Map<CreateOrderResponse>(order);
        }

        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<DeleteOrderResponse?> DeleteAsync(DeleteOrderRequest req, CancellationToken ct)
    {
        try
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "DeleteOrderRequest cannot be null");
            }
            if (req.Id <= 0)
            {
                throw new ArgumentException("Invalid order ID");
            }
            var order = await _orderRepository.FirstOrDefaultAsync(
                new OrderByIdWithIncludesSpec(req.Id), ct);
            if (order == null)
            {
                Log.Warning("Order with ID {Id} not found", req.Id);
                return null;
            }
            await _orderRepository.DeleteAsync(order, ct);
            return _mapper.Map<DeleteOrderResponse>(order);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null;
        }
    }

    public async Task<GetOrderByIdResponse?> GetByIdAsync(GetOrderByIdRequest req, CancellationToken ct)
    {
        try
        {
            var spec = new OrderByIdWithIncludesSpec(req.Id);
            var order = await _orderRepository.FirstOrDefaultAsync(spec, ct);
            if (order == null || order.IsDeleted)
                throw new Exception();
            return _mapper.Map<GetOrderByIdResponse>(order);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null;
        }
    }

    public async Task<OrderListResponse> ListAsync(CancellationToken ct)
    {
        try
        {
            var orders = await _orderRepository
                                .ListAsync(new OrdersWithIncludesSpec(), ct);
            var exsistingOrders = orders
                                .Where(o => !o.IsDeleted)
                                .ToList();
            var result = new OrderListResponse();
            result.Orders = _mapper.Map<List<OrderRecord>>(exsistingOrders);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<UpdateOrderResponse?> UpdateAsync(UpdateOrderRequest req, CancellationToken ct)
    {
        var spec = new OrderByIdWithIncludesSpec(req.Id);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, ct);
        if (order == null || order.IsDeleted)
            return null;
        order.OrderDate = req.OrderDate;
        order.UserId = req.UserId;
        order.ProductId = req.ProductId;
        await _orderRepository.UpdateAsync(order);
        return _mapper.Map<UpdateOrderResponse>(order);
    }

    public async Task<GetOrderLogsResponse> GetOrderLogsAsync(CancellationToken ct)
    {
        try
        {
            var spec = new OrderLogsSpec();
            var orderLogs = await _logRepository.ListAsync(spec, ct);
            var result = new GetOrderLogsResponse();
            result.OrderLogs = orderLogs;
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

}