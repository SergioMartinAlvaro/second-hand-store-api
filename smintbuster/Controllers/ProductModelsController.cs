﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smintbuster.Modals;

namespace smintbuster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductModelsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductModelsController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/ProductModels
        [HttpGet]
        public IEnumerable<ProductModel> GetProducts()
        {
            return _context.Products;
        }

        // GET: api/ProductModels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = await _context.Products.FindAsync(id);

            if (productModel == null)
            {
                return NotFound();
            }

            return Ok(productModel);
        }

        // PUT: api/ProductModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductModel([FromRoute] int id, [FromBody] ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productModel.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(productModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(id))
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

        // POST: api/ProductModels
        [HttpPost]
        public async Task<IActionResult> PostProductModel([FromBody] ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(productModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductModel", new { id = productModel.ProductId }, productModel);
        }

        // DELETE: api/ProductModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = await _context.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();

            return Ok(productModel);
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}