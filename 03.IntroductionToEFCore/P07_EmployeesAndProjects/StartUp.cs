namespace P07_EmployeesAndProjects
{
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using System;
    using System.Globalization;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Include(e => e.Manager)
                    .Where(e => e.EmployeesProjects
                            .Any(ep => ep.Project.StartDate.Year >= 2001
                                       && ep.Project.StartDate.Year <= 2003))
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        ManagerFirstName = e.Manager.FirstName,
                        ManagerLastName = e.Manager.LastName,
                        Projects = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ProjectName = ep.Project.Name,
                                StartDate = ep.Project.StartDate,
                                EndDate = ep.Project.EndDate
                            })
                    })
                    .Take(30)
                    .ToArray();

                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                    foreach (var project in employee.Projects)
                    {
                        Console.WriteLine($"--{project.ProjectName} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {project.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished"}");
                    }
                }
            }
        }
    }
}