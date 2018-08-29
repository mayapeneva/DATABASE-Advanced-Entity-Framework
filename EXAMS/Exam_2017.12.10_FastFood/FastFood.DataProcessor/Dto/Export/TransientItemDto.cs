namespace FastFood.DataProcessor.Dto.Export
{
    using Models;

    public class TransientItemDto
    {
        public string Name { get; set; }

        public Item MostPopularItem { get; set; }
    }
}