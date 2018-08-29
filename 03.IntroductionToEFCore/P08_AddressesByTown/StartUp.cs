namespace P08_AddressesByTown
{
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var addresses = context.Addresses
                    .Include(a => a.Town)
                    .Include(a => a.Employees)
                    .Select(a => new
                    {
                        AddressText = a.AddressText,
                        TownName = a.Town.Name,
                        EmployeeCount = a.Employees.Count
                    })
                    .OrderByDescending(a => a.EmployeeCount)
                    .ThenBy(a => a.TownName)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .ToArray();

                foreach (var address in addresses)
                {
                    Console.WriteLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
                }
            }
        }
    }
}