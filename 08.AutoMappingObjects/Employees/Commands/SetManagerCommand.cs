namespace Employees.Commands
{
    using Constants;
    using Data;
    using System;

    public class SetManagerCommand : AbstractCommand
    {
        public SetManagerCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var employeeId = int.Parse(arguments[0]);
            var managerId = int.Parse(arguments[1]);

            var employee = base.context.Employees.Find(employeeId);
            var manager = base.context.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new ArgumentException(Constants.InvalidId);
            }
            employee.ManagerId = managerId;

            manager.EmployeesToManage.Add(employee);

            base.context.SaveChanges();

            return $"Employee {manager.FirstName} {manager.LastName} was added as a manager to employee {employee.FirstName} {employee.LastName}";
        }
    }
}