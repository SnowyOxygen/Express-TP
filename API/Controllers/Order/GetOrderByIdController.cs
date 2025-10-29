using Business.Order;
using Business.Order.GetOrderByIdUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Order
{
    [ApiController, Route("api/order"), Tags("Order")]
    public class GetOrderByIdController(IGetOrderByIdUseCase useCase) : ControllerBase
    {
        private readonly IGetOrderByIdUseCase _useCase = useCase;

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetByIdAsync([FromRoute] long id)
        {
            OrderDto? orderDto = await _useCase.GetByIdAsync(id);
            if (orderDto == null)
            {
                return NotFound();
            }
            return Ok(orderDto);
        }
    }
}
