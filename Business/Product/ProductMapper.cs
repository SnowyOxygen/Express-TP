using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Category;

namespace Business.Product
{
    internal static class ProductMapper
    {
        internal static ProductDto ToDto(this Model.DAO.Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Stock = product.Stock,
                Category = product.Category?.ToDto()
            };
        }
    }
}
