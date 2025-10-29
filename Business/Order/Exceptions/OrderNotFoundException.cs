using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Order.Exceptions
{
    public class OrderNotFoundException(long orderId) 
        : Exception($"Order with ID {orderId} was not found.")
    {
    }
}
