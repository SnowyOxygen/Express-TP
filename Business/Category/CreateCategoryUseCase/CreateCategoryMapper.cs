using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Category.CreateCategoryUseCase
{
    internal static class CreateCategoryMapper
    {
        internal static Model.DAO.Category ToDAO(this CreateCategoryRequest request)
        {
            return new Model.DAO.Category
            {
                Title = request.Title,
                Description = request.Description,
                Color = request.Color
            };
        }
    }
}
