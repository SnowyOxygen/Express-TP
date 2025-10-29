using Business.Order.CancelOrderUseCase;
using Business.Order.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Order
{
    [ApiController, Route("api/orders"), Tags("Order")]
    public class CancelOrderController(ICancelOrderUseCase useCase) : ControllerBase
    {
        private readonly ICancelOrderUseCase _useCase = useCase;

        [HttpPut("{orderId}/cancel")]
        public async Task<ActionResult> CancelOrderAsync(long orderId)
        {
            try
            {
                await _useCase.CancelOrderAsync(orderId);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }
    }
}
