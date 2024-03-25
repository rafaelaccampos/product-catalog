using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalog.Dtos;
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
        public async Task<IActionResult> Create([FromBody] ProductInput productInput)
        {
            var product = new Product(
                productInput.Title, 
                productInput.Description, 
                productInput.Price,
                productInput.Category, 
                productInput.Owner);

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

            var productOutput = new ProductOutput
            {
                Id = id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                Owner = product.Owner
            };

            return Ok(productOutput);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.GetAll();

            var productsOutput = products.Select(p => new ProductOutput
            {
                Id = p.Id.ToString(),
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
                Owner = p.Owner
            }).ToList();

            return Ok(productsOutput);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] string category)
        {
            var product = await _repository.Update(new ObjectId(id), category);

            var productOutput = new ProductOutput
            {
                Id = product!.Id.ToString(),
                Title = product.Title,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                Owner = product.Owner
            };

            return Ok(productOutput);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.Delete(new ObjectId(id));

            return NoContent();
        }
    }
}
