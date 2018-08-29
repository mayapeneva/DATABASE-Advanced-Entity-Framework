namespace Employees.Commands
{
    using App.DTOs;
    using AutoMapper.QueryableExtensions;
    using Constants;
    using Data;
    using System;
    using System.Linq;

    public class EmployeeInfoCommand : AbstractCommand
    {
        public EmployeeInfoCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var employeeId = int.Parse(arguments[0]);
            var employee = base.context.Employees.Where(e => e.Id == employeeId)
                .ProjectTo<EmployeeDTO>()
                .SingleOrDefault();
            if (employee == null)
            {
                throw new ArgumentException(Constants.InvalidId);
            }

            return $"ID: {employee.Id} - {employee.FirstName} {employee.LastName} -  ${employee.Salary:f2}";
        }
    }
}