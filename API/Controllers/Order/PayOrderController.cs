using Business.Order.Exceptions;
using Business.Order.PayOrderUseCase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Order
{
    [ApiController, Route("api/orders"), Tags("Order")]
    public class PayOrderController(IPayOrderUseCase useCase) : ControllerBase
    {
        private readonly IPayOrderUseCase _useCase = useCase;

        [HttpPut("{orderId}/pay")]
        public async Task<ActionResult> PayOrderAsync(long orderId)
        {
            try
            {
                await _useCase.PayOrderAsync(orderId);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }
    }
}
