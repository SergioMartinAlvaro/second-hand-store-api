﻿using System;
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
    public class ShoppingCartModelsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ShoppingCartModelsController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCartModels
        [HttpGet]
        public IEnumerable<ShoppingCartModel> GetShoppingCarts()
        {
            return _context.ShoppingCarts;
        }

        // GET: api/ShoppingCartModels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingCartModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCartModel = await _context.ShoppingCarts.FindAsync(id);

            if (shoppingCartModel == null)
            {
                return NotFound();
            }

            return Ok(shoppingCartModel);
        }

        // PUT: api/ShoppingCartModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCartModel([FromRoute] int id, [FromBody] ShoppingCartModel shoppingCartModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoppingCartModel.ShoppingCartId)
            {
                return BadRequest();
            }

            _context.Entry(shoppingCartModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartModelExists(id))
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

        // POST: api/ShoppingCartModels
        [HttpPost]
        public async Task<IActionResult> PostShoppingCartModel([FromBody] ShoppingCartModel shoppingCartModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ShoppingCarts.Add(shoppingCartModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCartModel", new { id = shoppingCartModel.ShoppingCartId }, shoppingCartModel);
        }

        // DELETE: api/ShoppingCartModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCartModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCartModel = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCartModel == null)
            {
                return NotFound();
            }

            _context.ShoppingCarts.Remove(shoppingCartModel);
            await _context.SaveChangesAsync();

            return Ok(shoppingCartModel);
        }

        private bool ShoppingCartModelExists(int id)
        {
            return _context.ShoppingCarts.Any(e => e.ShoppingCartId == id);
        }
    }
}