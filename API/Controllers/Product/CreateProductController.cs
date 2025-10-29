using Business.Product;
using Business.Product.CreateProductUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Product
{
    [ApiController, Route("api/product"), Tags("Product")]
    public class CreateProductController(ICreateProductUseCase useCase) : ControllerBase
    {
        private readonly ICreateProductUseCase _useCase = useCase;

        [HttpPost, Route("")]
        public async Task<ActionResult<ProductDto>> CreateAsync(
            [FromBody] CreateProductRequest request)
        {
            try
            {
                return Ok(await _useCase.CreateAsync(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
