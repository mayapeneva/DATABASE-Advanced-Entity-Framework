namespace P09_Employee147
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
                var employee = context.Employees
                    .Where(e => e.EmployeeId == 147)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        Projects = e.EmployeesProjects
                            .Select(ep => new { ProjectName = ep.Project.Name })
                            .OrderBy(ep => ep.ProjectName)
                    })
                    .ToArray();

                foreach (var e in employee)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                    foreach (var project in e.Projects)
                    {
                        Console.WriteLine(project.ProjectName);
                    }
                }
            }
        }
    }
}