namespace CarDealer.App.Dtos
{
    using System;
    using System.Collections.Generic;

    public class OrderedCustomerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }

        public List<string> Sales { get; set; } = new List<string>();
    }
}