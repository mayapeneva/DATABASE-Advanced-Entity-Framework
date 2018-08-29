namespace FastFood.DataProcessor.Dto.Export
{
    using System.Collections.Generic;

    public class OrderByEmployeeDto
    {
        public string Customer { get; set; }

        public List<ItemExpDto> Items { get; set; }

        public decimal TotalPrice { get; set; }
    }
}