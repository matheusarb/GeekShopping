using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Models;
using GeekShopping.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ??
                        throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductVO>>> FindAll()
        {
            var products = await _productRepository.FindAll();
            if (products is null)
                return NotFound("Products not found");
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVO>> FindById(long id)
        {
            var product = await _productRepository.FindById(id);
            if (product is null) return NotFound("Product not found");
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductVO>> Create(ProductVO productVO)
        {
            try
            {
                if (productVO is null) return BadRequest("Product could not be created");
            
                var createdProd = await _productRepository.Create(productVO);
                return Created($"{createdProd.Id}", createdProd);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductVO>> Update(long id, ProductVO productVO)
        {
            try
            {
                if (productVO is null) return BadRequest("Product could not be updated");
            
                var updatedProd = await _productRepository.Update(id, productVO);
                return Ok(updatedProd);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var product = await _productRepository.FindById(id);
                if (product is null)
                    return BadRequest("Product not found");

                if(await _productRepository.Delete(product.Id))                
                    return Ok($"Product {product.Name} was deleted");

                return BadRequest("Could not delete product");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
