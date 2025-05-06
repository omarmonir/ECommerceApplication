using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Host.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController(ICategoryService categoryService) : ControllerBase
	{
		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var data = await categoryService.GetAllAsync();
			return data.Any() ? Ok(data) : NotFound(data);
		}

		[HttpGet("single/{id}")]
		public async Task<IActionResult> GetSingle(Guid id)
		{
			var data = await categoryService.GetByIdAsync(id);
			return data != null ? Ok(data) : NotFound(data);
		}

		[HttpPost("add")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Add(CreateCategory category)
		{
			if(!ModelState.IsValid) return BadRequest(ModelState);

			var result = await categoryService.AddAsync(category);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateCategory category)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await categoryService.UpdateAsync(category);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
		{
			var result = await categoryService.DeleteAsync(id);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("product-by-category/{categoryId}")]
        public async Task<IActionResult> GetProductByCategory(Guid categoryId)
		{
			var results = await categoryService.GetProductByCategory(categoryId);
			return results.Any() ? Ok(results) : NotFound();
		}

    }
}
