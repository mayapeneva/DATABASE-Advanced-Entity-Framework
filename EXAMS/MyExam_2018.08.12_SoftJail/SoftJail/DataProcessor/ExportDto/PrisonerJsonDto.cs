namespace SoftJail.DataProcessor.ExportDto
{
    using System;
    using System.Collections.Generic;
    using Data.Models;

    public class PrisonerJsonDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CellNumber { get; set; }

        public List<OfficerJsonDto> Officers { get; set; }

        public decimal TotalOfficerSalary { get; set; }
    }
}