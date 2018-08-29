﻿using System.Xml.Serialization;

namespace PetClinic.DataProcessor.DTOs.Export
{
    using System;
    using System.Collections.Generic;

    [XmlType("Procedure")]
    public class ProcedureExportDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public List<AnimalAidExportDto> AnimalAids { get; set; }

        public decimal TotalPrice { get; set; }
    }
}