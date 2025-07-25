using API.DTO;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("API/Stock")]
    [Authorize]
        
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var Tests = new TestClass();
            //await Tests.TestAsync();
            var stocks = await _context.Stocks.Include(c => c.Comments).ToListAsync();
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
            
            if (stock == null) {
                return NotFound();
            }

            return Ok(stock);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByNames([FromQuery] string? CompanyName, [FromQuery] string? Symbol, [FromQuery] bool? IsDesc, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=2 )
        {
            var stockqry = _context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!CompanyName.IsNullOrEmpty()) 
            {
                stockqry = stockqry.Where(s => s.CompanyName.Contains(CompanyName));
            }
            if (!Symbol.IsNullOrEmpty())
            {
                stockqry = stockqry.Where(s => s.Symbol.Contains(Symbol));
            }
            if (!IsDesc.HasValue)
            {
                stockqry = IsDesc.GetValueOrDefault() ? stockqry.OrderByDescending(s => s.Symbol) : stockqry.OrderBy(s => s.Symbol);
            }
                            
            var stock = await stockqry.Skip((PageNumber -1) * PageSize).Take(PageSize).ToListAsync();
            return Ok(stock);
         }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockDTO stockDTO) {
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = new Stock
            {
                Symbol = stockDTO.Symbol,
                CompanyName = stockDTO.CompanyName,
                MarketCap = stockDTO.MarketCap,
                Industry = stockDTO.Industry,
                LastDiv = stockDTO.LastDiv,
                Purchase = stockDTO.Purchase
            };

            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] StockDTO stockDTO) {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }

            stock.Symbol = stockDTO.Symbol;
            stock.CompanyName = stockDTO.CompanyName;
            stock.Purchase = stockDTO.Purchase;
            stock.MarketCap = stockDTO.MarketCap;
            stock.LastDiv = stockDTO.LastDiv;
            stock.Industry = stockDTO.Industry;

            _context.SaveChanges();

            return NoContent();
        
        }

        [HttpPatch ("{id}")]
        public IActionResult PartUpdate(int id, [FromBody] StockDTOPatch stockDTO)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }

            if (stockDTO.Purchase.HasValue) stock.Purchase = stockDTO.Purchase.Value;
            if (stockDTO.MarketCap.HasValue) stock.MarketCap = stockDTO.MarketCap.Value;
            if (stockDTO.LastDiv.HasValue) stock.LastDiv = stockDTO.LastDiv.Value;

            _context.SaveChanges();

            return Ok("Stock updated sucessfully!");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id) { 
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stock);

            _context.SaveChanges(); 
            
            return Ok("Stock deleted successfully!"); 
        }


    }
}
