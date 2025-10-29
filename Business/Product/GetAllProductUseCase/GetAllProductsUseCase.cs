using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repositories.Interfaces;

namespace Business.Product.GetAllProductUseCase
{
    public class GetAllProductsUseCase(IProductRepository repository) : IGetAllProductsUseCase
    {
        private readonly IProductRepository _repository = repository;

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            IEnumerable<Model.DAO.Product> products = await _repository.GetAllAsync();

            return products.Select(p => p.ToDto());
        }
    }
}
