namespace P14_DeleteProjectById
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
                var employeeProject = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);
                context.EmployeesProjects.RemoveRange(employeeProject);

                var project = context.Projects.Find(2);
                context.Projects.Remove(project);

                context.SaveChanges();

                var projects = context.Projects
                    .Select(p => p.Name)
                    .Take(10)
                    .ToArray();

                Console.WriteLine(string.Join(Environment.NewLine, projects));
            }
        }
    }
}