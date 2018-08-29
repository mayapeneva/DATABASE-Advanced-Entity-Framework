namespace P10_DeptsWithMoreThan5Emps
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var departments = context.Departments
                    .Include(d => d.Employees)
                    .Where(d => d.Employees.Count > 5)
                    .Select(d => new
                    {
                        DepartmentName = d.Name,
                        MangerFirstName = d.Manager.FirstName,
                        ManagerLastName = d.Manager.LastName,
                        Employees = d.Employees
                            .Select(e => new
                            {
                                EmployeeFirstName = e.FirstName,
                                EmployeeLastName = e.LastName,
                                EmployeeJobTitle = e.JobTitle
                            })
                            .OrderBy(e => e.EmployeeFirstName)
                            .ThenBy(e => e.EmployeeLastName)
                    })
                    .OrderBy(d => d.Employees.Count())
                    .ThenBy(d => d.DepartmentName)
                    .ToArray();

                foreach (var department in departments)
                {
                    Console.WriteLine($"{department.DepartmentName} - {department.MangerFirstName} {department.ManagerLastName}");

                    foreach (var employee in department.Employees)
                    {
                        Console.WriteLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - {employee.EmployeeJobTitle}");
                    }

                    Console.WriteLine(new string('-', 10));
                }
            }
        }
    }
}