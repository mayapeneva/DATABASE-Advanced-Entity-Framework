namespace P15_RemoveTowns
{
    using System;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var townName = Console.ReadLine();

                var addresses = context.Addresses
                    .Where(a => a.Town.Name == townName)
                    .ToArray();
                var addressesNumber = addresses.Length;

                var employees = context.Employees
                    .Where(e => addresses.Select(a => a.AddressId).Any(id => id == e.AddressId));
                foreach (var employee in employees)
                {
                    employee.AddressId = null;
                }

                context.Addresses.RemoveRange(addresses);

                var town = context.Towns.FirstOrDefault(t => t.Name == townName);
                if (town != null)
                {
                    context.Towns.Remove(town);
                }

                context.SaveChanges();

                if (addressesNumber == 1)
                {
                    Console.WriteLine($"{addressesNumber} address in {townName} was deleted");
                }
                else
                {
                    Console.WriteLine($"{addressesNumber} address in {townName} were deleted");
                }
            }
        }
    }
}