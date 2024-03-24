using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

            return Created(nameof(GetCategoryId), null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryId(string id)
        {
            var category = await _repository.GetCategoryById(new ObjectId(id));

            if(category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _repository.GetAll();

            return Ok(categories);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.Delete(new ObjectId(id));

            return NoContent();
        }
    }
}
