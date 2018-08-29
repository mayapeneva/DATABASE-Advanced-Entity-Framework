namespace SoftJail.DataProcessor
{
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .Select(p => new PrisonerJsonDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                        .OrderBy(po => po.Officer.FullName)
                        .ThenBy(po => po.Officer.Id)
                        .Select(po => new OfficerJsonDto
                        {
                            OfficerName = po.Officer.FullName,
                            Department = po.Officer.Department.Name
                        }).ToList(),
                    TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary)
                })
                .ToArray();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prissNames = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var prisoners = context.Prisoners
                .Where(p => prissNames.Contains(p.FullName))
                .OrderBy(p => p.FullName)
                .OrderBy(p => p.Id)
                .Select(p => new PrisonerXMLDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new MessageXMLDto
                    {
                        Description = Reverse(m.Description)
                    }).ToList()
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(PrisonerXMLDto[]), new XmlRootAttribute("Prisoners"));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, prisoners, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return writer.ToString();
            }
        }

        private static string Reverse(string text)
        {
            var arr = text.ToCharArray();
            Array.Reverse(arr);
            return new String(arr);
        }
    }
}