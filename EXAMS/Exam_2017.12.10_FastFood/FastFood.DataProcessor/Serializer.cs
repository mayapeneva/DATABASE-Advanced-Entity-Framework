using System;
using System.IO;
using FastFood.Data;

namespace FastFood.DataProcessor
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Xml;
    using System.Xml.Serialization;
    using Dto.Export;
    using Models.Enums;
    using Newtonsoft.Json;
    using Remotion.Linq.Parsing.Structure.IntermediateModel;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var type = Enum.Parse<OrderType>(orderType);
            var employee = context.Employees
                .ToArray()
                .Where(e => e.Name == employeeName)
                .Select(e => new EmployeeExpDto
                {
                    Name = e.Name,
                    Orders = e.Orders.Where(o => o.Type == type)
                        .Select(o => new OrderByEmployeeDto
                        {
                            Customer = o.Customer,
                            Items = o.OrderItems.Select(oi => new ItemExpDto
                            {
                                Name = oi.Item.Name,
                                Price = oi.Item.Price,
                                Quantity = oi.Quantity
                            }).ToList(),
                            TotalPrice = o.TotalPrice
                        })
                        .OrderByDescending(o => o.TotalPrice)
                        .ThenByDescending(o => o.Items.Count)
                        .ToList(),
                    TotalMade = e.Orders.Where(o => o.Type == type).Sum(o => o.TotalPrice)
                })
                .FirstOrDefault();

            return JsonConvert.SerializeObject(employee, Formatting.Indented);
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var wantedCategories = categoriesString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var categories = context.Categories
                .Where(c => wantedCategories.Any(wc => wc == c.Name))
                .Select(c => new CategoryDto
                {
                    Name = c.Name,
                    MostPopularItem = c.Items.Select(i => new ItemCategoryDto()
                    {
                        Name = i.Name,
                        TotalMade = i.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity),
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                    })
                            .OrderByDescending(i => i.TotalMade)
                            .ThenByDescending(i => i.TimesSold)
                            .FirstOrDefault()
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, categories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return writer.ToString();
            }
        }
    }
}