// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using OrderApp.Infrastructure.Data;

// namespace OrderApp.Web.Products;

// [ApiController]
// [Route("api/[controller]")]
// public class ProductsController : ControllerBase
// {
//     private readonly AppDbContext _context;
//     public ProductsController(AppDbContext context)
//     {
//         _context = context;
//     }
//     [Microsoft.AspNetCore.Mvc.HttpGet]
//     public async Task<IActionResult> Get()
//     {
//         var products = await _context.Products.ToListAsync();
//         return Ok(products);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
//     public async Task<IActionResult> GetById([FromRoute]int id)
//     {
//         var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
//         if (product == null)
//         {
//             return NotFound();
//         }
//         return Ok(product);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpPost]
//     public async Task<IActionResult> Create([Microsoft.AspNetCore.Mvc.FromBody] Core.ProductAggregate.Product product)
//     {
//         if (product == null)
//         {
//             return BadRequest("Product cannot be null");
//         }
//         _context.Products.Add(product);
//         await _context.SaveChangesAsync();
//         return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
//     public async Task<IActionResult> Update([FromRoute]int id, [Microsoft.AspNetCore.Mvc.FromBody] Core.ProductAggregate.Product product)
//     {
//         if (id != product.Id)
//         {
//             return BadRequest("Product ID mismatch");
//         }
//         var existingProduct = await _context.Products.FindAsync(id);
//         if (existingProduct == null)
//         {
//             return NotFound();
//         }
//         existingProduct.Name = product.Name;
//         existingProduct.Price = product.Price;
//         existingProduct.CompanyId = product.CompanyId;
//         _context.Products.Update(existingProduct);
//         await _context.SaveChangesAsync();
//         return Ok();
//     }

//     [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
//     public async Task<IActionResult> Delete([FromRoute]int id)
//     {
//         var product = await _context.Products.FindAsync(id);
//         if (product == null)
//         {
//             return NotFound();
//         }
//         _context.Products.Remove(product);
//         await _context.SaveChangesAsync();
//         return NoContent();
//     }
// }
