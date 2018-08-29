using System;
using FastFood.Data;

namespace FastFood.DataProcessor
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Dto.Import;
    using Models;
    using Models.Enums;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var positions = new List<Position>();
            var employees = new List<Employee>();
            var result = new StringBuilder();

            var objEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);
            foreach (var objEmployee in objEmployees)
            {
                if (!IsValid(objEmployee) || objEmployee.Position == "Invalid")
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                Position position = null;
                if (positions.All(p => p.Name != objEmployee.Position))
                {
                    position = new Position { Name = objEmployee.Position };
                    positions.Add(position);
                }
                else
                {
                    position = positions.First(p => p.Name == objEmployee.Position);
                }

                var employee = new Employee
                {
                    Name = objEmployee.Name,
                    Age = objEmployee.Age,
                    Position = position
                };

                employees.Add(employee);
                result.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var categories = new List<Category>();
            var items = new List<Item>();
            var result = new StringBuilder();

            var objItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);
            foreach (var objItem in objItems)
            {
                var ifItemExists = items.Any(i => i.Name == objItem.Name);
                if (!IsValid(objItem) || ifItemExists
                    || objItem.Name.Length < 3 || objItem.Category.Length < 3
                    || objItem.Name == "Invalid" || objItem.Category == "Invalid")
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                Category category = null;
                if (categories.All(p => p.Name != objItem.Category))
                {
                    category = new Category { Name = objItem.Category };
                    categories.Add(category);
                }
                else
                {
                    category = categories.First(p => p.Name == objItem.Category);
                }

                var item = new Item
                {
                    Name = objItem.Name,
                    Category = category,
                    Price = objItem.Price
                };

                items.Add(item);
                result.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(items);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var orders = new List<Order>();
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var objOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));
            foreach (var objOrder in objOrders)
            {
                var ifItemValid = true;

                var employee = context.Employees.FirstOrDefault(e => e.Name == objOrder.Employee);
                if (!IsValid(objOrder) || employee == null)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var dateTime = DateTime.ParseExact(objOrder.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var type = Enum.Parse<OrderType>(objOrder.Type);
                var order = new Order
                {
                    Customer = objOrder.Customer,
                    DateTime = dateTime,
                    Type = type,
                    Employee = employee
                };

                foreach (var dtoItem in objOrder.OrderItems)
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == dtoItem.Name);
                    if (!IsValid(dtoItem) || item == null)
                    {
                        result.AppendLine(FailureMessage);
                        ifItemValid = false;
                        break;
                    }

                    var orderItem = new OrderItem
                    {
                        Item = item,
                        Order = order,
                        Quantity = dtoItem.Quantity
                    };
                    order.OrderItems.Add(orderItem);
                }

                if (!ifItemValid)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                orders.Add(order);
                result.AppendLine($"Order for {order.Customer} on {order.DateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(orders);
            //context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            var vContext = new ValidationContext(obj);
            var vResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, vContext, vResults, true);
        }
    }
}