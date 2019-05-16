using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smintbuster.Modals;

namespace smintbuster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopContext _context;

        public CategoryController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        [Authorize]
        public IEnumerable<CategoryModel> GetCategories()
        {
            var categories = _context.Categories;
            foreach(var categoryModel in categories)
            {
                IEnumerable<ProductModel> Products = GetProductsByCategoryId(categoryModel.CategoryId);

                categoryModel.Products = Products.ToList<ProductModel>();
            }
            return categories;
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryModel = await _context.Categories.FindAsync(id);

            if (categoryModel == null)
            {
                return NotFound();
            }

            IEnumerable<ProductModel> Products = GetProductsByCategoryId(categoryModel.CategoryId);

            categoryModel.Products = Products.ToList<ProductModel>();

            return Ok(categoryModel);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCategoryModel([FromRoute] int id, [FromBody] CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoryModel.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(categoryModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryModelExists(id))
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

        // POST: api/Category
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostCategoryModel([FromBody] CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Categories.Add(categoryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryModel", new { id = categoryModel.CategoryId }, categoryModel);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategoryModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryModel = await _context.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryModel);
            await _context.SaveChangesAsync();

            return Ok(categoryModel);
        }

        private bool CategoryModelExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        private dynamic GetProductsByCategoryId(int id)
        {
            return from b in _context.Products
                        where b.CategoryId.Equals(id)
                        select new ProductModel {
                            ProductId = b.ProductId,
                            ProductName = b.ProductName,
                            ProductDescription = b.ProductDescription,
                            ProductImage = b.ProductImage,
                            ProductPrice = b.ProductPrice,
                            CategoryId = b.CategoryId,
                            UserId = b.UserId
                        };
        }
    }
}