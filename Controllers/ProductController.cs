using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NG_Core_Auth.Data;
using NG_Core_Auth.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NG_Core_Auth.Controllers
{
    [Route("api/[controller]")] 
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


    
        [HttpGet("[action]")]
        [Authorize(Policy = "RequiredLoggedIn")]
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.ToList());
        }



        [HttpGet("[action]/{id}")]
        [Authorize(Policy = "RequiredAdministratorRole")]
        public IActionResult GetProduct(int id)
        {
            var findProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (findProduct == null)
            {
                return NotFound();
            }


            return Ok(findProduct);
        }

        [Authorize(Policy = "RequiredAdministratorRole")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel formData)
        {
            var newProduct = new ProductModel()
            {
                Name = formData.Name,
                Description = formData.Description,
                OutOfStock = Convert.ToBoolean(formData.OutOfStock),
                ImageUrl = formData.ImageUrl,
                Price = formData.Price

            };

           await _context.Products.AddAsync(newProduct);

           await _context.SaveChangesAsync();

            return Ok(new JsonResult("The Product was Added Successfully"));
        }

        [Authorize(Policy = "RequiredAdministratorRole")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductModel formData)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (findProduct == null)
            {
                return NotFound();
            }

            findProduct.Name = formData.Name;
            findProduct.Description = formData.Description;
            findProduct.ImageUrl = formData.ImageUrl;
            findProduct.OutOfStock = Convert.ToBoolean(formData.OutOfStock);
            findProduct.Price = formData.Price;

            _context.Entry(findProduct).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new JsonResult("The Product with id " + id + " has been Updated"));
        }

        [Authorize(Policy = "RequiredAdministratorRole")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var findProduct = await _context.Products.FindAsync(id);
            if (findProduct == null)
            {
                return NotFound();
            }

            _context.Products.Remove(findProduct);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult("The Product with id " + id + " has been Deleted"));
        }

    }
}
