using Business.Product;
using Business.Product.GetAllProductUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Product
{
    [ApiController, Route("api/product"), Tags("Product")]
    public class GetAllProductsController(IGetAllProductsUseCase useCase) : ControllerBase
    {
        private readonly IGetAllProductsUseCase _useCase = useCase;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync()
        {
            IEnumerable<ProductDto> products = await _useCase.GetAllAsync();

            return Ok(products);
        }
    }
}
