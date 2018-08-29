namespace Employees.Commands
{
    using App.DTOs;
    using AutoMapper.QueryableExtensions;
    using Constants;
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class EmployeePersonalInfoCommand : AbstractCommand
    {
        public EmployeePersonalInfoCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var employeeId = int.Parse(arguments[0]);
            var employee = base.context.Employees.Where(e => e.Id == employeeId)
                .ProjectTo<EmployeeAllInfoDTO>()
                .SingleOrDefault();
            if (employee == null)
            {
                throw new ArgumentException(Constants.InvalidId);
            }

            var result = new StringBuilder();
            result.AppendLine($"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}");
            if (employee.Birthday != null)
            {
                result.AppendLine($"Birthday: {employee.Birthday.Value:dd-MM-yyyy}");
            }
            result.AppendLine($"Address: {employee.Address}");

            return result.ToString().Trim();
        }
    }
}