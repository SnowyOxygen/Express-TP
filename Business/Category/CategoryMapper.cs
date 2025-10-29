namespace Business.Category
{
    internal static class CategoryMapper
    {
        internal static CategoryDto ToDto(this Model.DAO.Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description,
                Color = category.Color
            };
        }
    }
}
