namespace P05_EmployeesFromRAD
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
                var employees = context.Employees.Include(e => e.Department)
                    .Where(e => e.Department.Name == "Research and Development").Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        DepartmentName = e.Department.Name,
                        e.Salary
                    }).OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName);

                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
                }
            }
        }
    }
}