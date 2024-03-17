using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalog.Entities;
using ProductCatalog.Infra.Repositories;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            await _repository.Create(product);

            return Created(nameof(GetProductById), null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _repository.GetProductById(new ObjectId(id));

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.GetAll();

            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] string category)
        {
            await _repository.Update(new ObjectId(id), category);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.Delete(new ObjectId(id));

            return NoContent();
        }
    }
}
