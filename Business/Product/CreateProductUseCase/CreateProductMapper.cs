using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Product.CreateProductUseCase
{
    internal static class CreateProductMapper
    {
        public static Model.DAO.Product ToDao(this CreateProductRequest request)
            => new()
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                SalePrice = request.SalePrice,
                Stock = request.Stock,
                CategoryId = request.CategoryId,
                SaleActive = false
            };
    }
}

