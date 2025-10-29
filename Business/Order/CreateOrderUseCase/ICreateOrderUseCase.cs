using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Order.CreateOrderUseCase
{
    public interface ICreateOrderUseCase
    {
        Task<long> CreateAsync(CreateOrderRequest request);
    }
}
