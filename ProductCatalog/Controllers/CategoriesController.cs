using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Entities;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository _repository;

        public CategoriesController(CategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            await _repository.Create(category);

            return Ok();
        }
    }
}
