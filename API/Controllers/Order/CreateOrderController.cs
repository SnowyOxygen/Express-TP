using Business.Order;
using Business.Order.CreateOrderUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Order
{
    [ApiController, Route("api/order"), Tags("Order")]
    public class CreateOrderController(ICreateOrderUseCase useCase) : ControllerBase
    {
        private readonly ICreateOrderUseCase _useCase = useCase;

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] CreateOrderRequest request)
        {
            long createdId = await _useCase.CreateAsync(request);

            return Ok(createdId);
        }
    }
}
