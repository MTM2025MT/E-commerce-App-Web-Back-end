using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_App_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productRepository.GetAllProducts();
                return Ok(products);

            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProductById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            var product = _productRepository.GetProduct(id);

            if (product == null)
                return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpGet("{category:alpha}")]
        public IActionResult GetProductsByCategory(Category category)
        {
            
            var products = _productRepository.GetProductsByCategory(category);
            if(products == null)
            {
                return NotFound("No products found in this category.");
            }
            return Ok(products);
        }
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            try { 
                if (product == null)
                {
                    return BadRequest("Product cannot be null.");
                }
                if(ModelState.IsValid == false)
                {
                    return BadRequest("Invalid product data.");
                }
                var res= _productRepository.AddProduct(product);
                var procutresult =new CreatedResult("",res);
                return Created();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
        [HttpPut("UpdateProduct/{id:int}")]
        public async Task<IActionResult> EditProduct(int id, Product product)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            try
            {
                var updatedProduct = await _productRepository.EditProduct(id, product);

                if (updatedProduct == null)
                    return NotFound("Product not found.");

                return Ok(updatedProduct);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("The product was modified by another user.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Could not update the product due to database rules.");
            }

            catch (Exception)
            {
                return StatusCode(500, "Something went wrong.");
            }
        }



    }

}
