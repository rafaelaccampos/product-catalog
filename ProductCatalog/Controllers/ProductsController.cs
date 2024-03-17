using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Infra.Repositories;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MongoContext _context;

        public ProductsController(MongoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var productRepository = new ProductRepository(_context);
            await productRepository.Create(product);

            return Created(nameof(GetProductById), null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var productRepository = new ProductRepository(_context);
            var product = await productRepository.GetProductById(new ObjectId(id));

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productRepository = new ProductRepository(_context);
            var products = await productRepository.GetAll();

            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] string category)
        {
            var productRepository = new ProductRepository(_context);
            await productRepository.Update(new ObjectId(id), category);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var productRepository = new ProductRepository(_context);
            await productRepository.Delete(new ObjectId(id));

            return NoContent();
        }
    }
}
