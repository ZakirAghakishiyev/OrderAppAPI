// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using OrderApp.Infrastructure.Data;

// namespace OrderApp.Web.Companies;

// [ApiController]
// [Route("api/[controller]")]
// public class CompaniesController : ControllerBase
// {
//     private readonly AppDbContext _context;

//     public CompaniesController(AppDbContext context)
//     {
//         _context = context;
//     }

//     [Microsoft.AspNetCore.Mvc.HttpGet]
//     public async Task<IActionResult> Get()
//     {
//         var companies = await _context.Companies.ToListAsync();
//         return Ok(companies);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
//     public async Task<IActionResult> GetById([FromRoute]int id)
//     {
//         var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
//         if (company == null)
//         {
//             return NotFound();
//         }
//         return Ok(company);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpPost]
//     public async Task<IActionResult> Create([Microsoft.AspNetCore.Mvc.FromBody] Core.CompanyAggregate.Company company)
//     {
//         if (company == null)
//         {
//             return BadRequest("Company cannot be null");
//         }
//         _context.Companies.Add(company);
//         await _context.SaveChangesAsync();
//         return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
//     }

//     [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, string name)
//     {
//     //     if (company.Id != company.Id)
//     //     {
//     //         return BadRequest("Company ID mismatch");
//     //     }
//         var existingCompany = await _context.Companies.FindAsync(id);
//         if (existingCompany == null)
//         {
//             return NotFound();
//         }
//         existingCompany.Name = name;
//         _context.Companies.Update(existingCompany);
//         await _context.SaveChangesAsync();
//         return Ok();
//     }

//     [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
//     public async Task<IActionResult> Delete([FromRoute] int id)
//     {
//         var company = await _context.Companies.FindAsync(id);
//         if (company == null)
//         {
//             return NotFound();
//         }
//         _context.Companies.Remove(company);
//         await _context.SaveChangesAsync();
//         return NoContent();
//     }

// }

