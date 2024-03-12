using Microsoft.AspNetCore.Mvc;
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

            return Ok(product);
        }
    }
}
