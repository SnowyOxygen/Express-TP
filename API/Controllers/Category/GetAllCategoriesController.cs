using Business.Category;
using Business.Category.GetAllCategoriesUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Category
{
    [ApiController, Route("api/category"), Tags("Category")]
    public class GetAllCategoriesController(IGetAllCategoriesUseCase useCase) : ControllerBase
    {
        [HttpGet, Route("")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryDto> categories = await useCase.GetAllAsync();

            return Ok(categories);
        }
    }
}
