using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalog.Dtos;
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
        public async Task<IActionResult> Create([FromBody] CategoryInput categoryInput)
        {
            var category = new Category(
                categoryInput.Title,
                categoryInput.Description,
                categoryInput.Owner);

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

            var categoryOutput = new CategoryOutput
            {
                Id = category.Id.ToString(),
                Title = category.Title,
                Description = category.Description,
                Owner = category.Owner,
            };

            return Ok(categoryOutput);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _repository.GetAll();

            var categoriesOutput = categories.Select(p => new CategoryOutput
            {
                Id = p!.Id.ToString(),
                Title = p.Title,
                Description = p.Description,
                Owner = p.Owner,
            }).ToList();

            return Ok(categoriesOutput);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.Delete(new ObjectId(id));

            return NoContent();
        }
    }
}
