namespace Employees.Commands
{
    using App.DTOs;
    using AutoMapper.QueryableExtensions;
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class ListEmployeesOlderThanCommand : AbstractCommand
    {
        public ListEmployeesOlderThanCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var age = int.Parse(arguments[0]);
            var targetDate = new DateTime(DateTime.Now.Year - age, DateTime.Now.Month, DateTime.Now.Day);

            var employees = base.context.Employees.Where(e => Nullable.Compare<DateTime>(targetDate, e.Birthday) > 0)
                .ProjectTo<EmployeeWithManagerDTO>().OrderByDescending(e => e.Salary);

            var result = new StringBuilder();
            foreach (var employee in employees)
            {
                var managerName = employee.Manager != null ? employee.Manager.LastName : "";
                result.AppendLine($"{employee.FirstName} {employee.LastName} - ${employee.Salary:F2} - Manager: {managerName}");
            }

            return result.ToString().Trim();
        }
    }
}