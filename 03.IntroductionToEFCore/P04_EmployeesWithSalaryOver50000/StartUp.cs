namespace P04_EmployeesWithSalaryOver50000
{
    using P02_DatabaseFirst.Data;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var employees = context.Employees.Where(e => e.Salary > 50000).Select(e => new { e.FirstName })
                    .OrderBy(e => e.FirstName);

                Console.WriteLine(string.Join(Environment.NewLine, employees.Select(e => e.FirstName)));
            }
        }
    }
}