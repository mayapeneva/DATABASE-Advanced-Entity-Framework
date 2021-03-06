﻿namespace CarDealer.Models
{
    using System.Collections.Generic;

    public class Car
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public virtual ICollection<PartCar> Parts { get; set; } = new HashSet<PartCar>();
    }
}