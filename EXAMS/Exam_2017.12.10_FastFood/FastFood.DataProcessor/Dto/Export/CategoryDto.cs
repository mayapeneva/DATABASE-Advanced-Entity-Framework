namespace FastFood.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;
    using Models;

    [XmlType("Category")]
    public class CategoryDto
    {
        public string Name { get; set; }

        public ItemCategoryDto MostPopularItem { get; set; }
    }
}