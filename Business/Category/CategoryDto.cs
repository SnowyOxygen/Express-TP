namespace Business.Category
{
    public sealed class CategoryDto
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Color { get; set; }
    }
}
