// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using OrderApp.Infrastructure.Data;
// using OrderApp.Core.OrderAggregate;
// namespace OrderApp.Web.Orders;

// [ApiController]
// [Route("api/[controller]")]
// public class OrdersController : ControllerBase
// {
//     private readonly AppDbContext _context;

//     public OrdersController(AppDbContext context)
//     {
//         _context = context;
//     }

//     [Microsoft.AspNetCore.Mvc.HttpGet]
//     public async Task<IActionResult> GetOrders()
//     {
//         var orders = await _context.Orders.ToListAsync();
//         return Ok(orders);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
//     public async Task<IActionResult> GetOrderById([FromRoute]int id)
//     {
//         var order = await _context.Orders.FindAsync(id);
//         if (order == null)
//         {
//             return NotFound();
//         }
//         return Ok(order);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpPost]
//     public async Task<IActionResult> CreateOrder([Microsoft.AspNetCore.Mvc.FromBody] Core.OrderAggregate.Order order)
//     {
//         if (order == null)
//         {
//             return BadRequest("Order cannot be null");
//         }

//         _context.Orders.Add(order);
//         await _context.SaveChangesAsync();
//         return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
//     public async Task<IActionResult> UpdateOrder([FromRoute]int id, [Microsoft.AspNetCore.Mvc.FromBody] Core.OrderAggregate.Order order)
//     {
//         if (id != order.Id)
//         {
//             return BadRequest("Order ID mismatch");
//         }

//         var existingOrder = await _context.Orders.FindAsync(id);
//         if (existingOrder == null)
//         {
//             return NotFound();
//         }

//         existingOrder.OrderDate = order.OrderDate;
//         existingOrder.UserId = order.UserId;
//         existingOrder.ProductId = order.ProductId;

//         _context.Orders.Update(existingOrder);
//         await _context.SaveChangesAsync();
//         return NoContent();
//     }

//     [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteOrder([FromRoute]int id)
//     {
//         var order = await _context.Orders.FindAsync(id);
//         if (order == null)
//         {
//             return NotFound();
//         }

//         _context.Orders.Remove(order);
//         await _context.SaveChangesAsync();
//         return NoContent();
//     }
// }