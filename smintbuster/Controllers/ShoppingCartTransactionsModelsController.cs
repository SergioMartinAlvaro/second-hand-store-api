using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smintbuster.Modals;
using smintbuster.Models;

namespace smintbuster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartTransactionsModelsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ShoppingCartTransactionsModelsController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCartTransactionsModels
        [HttpGet]
        public IEnumerable<ShoppingCartTransactionsModel> GetShoppingCartTransactions()
        {
            return _context.ShoppingCartTransactions;
        }

        // GET: api/ShoppingCartTransactionsModels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingCartTransactionsModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCartTransactionsModel = await _context.ShoppingCartTransactions
                .Join(_context.Products,
                    shop => shop.ProductId,
                    product => product.ProductId,
                    (shop, product) => new
                    {
                        Id = shop.Id,
                        ShoppingCartId = shop.ShoppingCartId,
                        ProductImage = product.ProductImage,
                        ProductId = product.ProductId,
                        UserId = product.UserId,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice
                    }
                ).Where(p => p.ShoppingCartId == id).Select(p => p).ToListAsync();

            if (shoppingCartTransactionsModel == null)
            {
                return NotFound();
            }

            return Ok(shoppingCartTransactionsModel);
        }

        // PUT: api/ShoppingCartTransactionsModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCartTransactionsModel([FromRoute] int id, [FromBody] ShoppingCartTransactionsModel shoppingCartTransactionsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoppingCartTransactionsModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(shoppingCartTransactionsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartTransactionsModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ShoppingCartTransactionsModels
        [HttpPost]
        public async Task<IActionResult> PostShoppingCartTransactionsModel([FromBody] ShoppingCartTransactionsModel shoppingCartTransactionsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ShoppingCartTransactions.Add(shoppingCartTransactionsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCartTransactionsModel", new { id = shoppingCartTransactionsModel.Id }, shoppingCartTransactionsModel);
        }

        // DELETE: api/ShoppingCartTransactionsModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCartTransactionsModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCartTransactionsModel = await _context.ShoppingCartTransactions.FindAsync(id);
            if (shoppingCartTransactionsModel == null)
            {
                return NotFound();
            }

            _context.ShoppingCartTransactions.Remove(shoppingCartTransactionsModel);
            await _context.SaveChangesAsync();

            return Ok(shoppingCartTransactionsModel);
        }

        private bool ShoppingCartTransactionsModelExists(int id)
        {
            return _context.ShoppingCartTransactions.Any(e => e.Id == id);
        }
    }
}