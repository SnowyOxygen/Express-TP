using Business.Category;
using Business.Category.CreateCategoryUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Category
{
    [ApiController, Route("api/category"), Tags("Category")]
    public class CreateCategoryController(ICreateCategoryUseCase useCase) : ControllerBase
    {
        private readonly ICreateCategoryUseCase _useCase = useCase;

        [HttpPost, Route("create")]
        public async Task<ActionResult<CategoryDto>> CreateCategoryAsync(
            [FromBody] CreateCategoryRequest request)
        {
            try
            {
                CategoryDto createdCategory = await _useCase.CreateAsync(request);

                return Ok(createdCategory);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
