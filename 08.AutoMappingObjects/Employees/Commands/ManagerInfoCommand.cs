namespace Employees.Commands
{
    using App.DTOs;
    using AutoMapper.QueryableExtensions;
    using Constants;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;

    public class ManagerInfoCommand : AbstractCommand
    {
        public ManagerInfoCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var managerId = int.Parse(arguments[0]);
            var manager = base.context.Employees.Include(e => e.Manager).Where(e => e.Id == managerId)
                .ProjectTo<ManagerDTO>()
                .SingleOrDefault();
            if (manager == null)
            {
                throw new ArgumentException(Constants.InvalidId);
            }

            var result = new StringBuilder();
            result.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.EmployeesToManage.Count}");
            foreach (var employee in manager.EmployeesToManage)
            {
                result.AppendLine($"- {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}");
            }

            return result.ToString().Trim();
        }
    }
}