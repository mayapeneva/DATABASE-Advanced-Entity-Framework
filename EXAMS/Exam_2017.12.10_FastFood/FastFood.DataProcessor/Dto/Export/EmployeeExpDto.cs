namespace FastFood.DataProcessor.Dto.Export
{
    using System.Collections.Generic;

    public class EmployeeExpDto
    {
        public string Name { get; set; }

        public List<OrderByEmployeeDto> Orders { get; set; }

        public decimal TotalMade { get; set; }
    }
}