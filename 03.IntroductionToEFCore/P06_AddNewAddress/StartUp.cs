namespace P06_AddNewAddress
{
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var address = new Address
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };
                context.Addresses.Update(address);

                var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
                if (employee != null)
                {
                    employee.Address = address;
                    context.Employees.Update(employee);
                }

                context.SaveChanges();

                var employees = context.Employees.Include(e => e.Address).OrderByDescending(e => e.AddressId).Take(10)
                    .Select(e => new { AddressText = e.Address.AddressText });

                Console.WriteLine(string.Join(Environment.NewLine, employees.Select(e => e.AddressText)));
            }
        }
    }
}