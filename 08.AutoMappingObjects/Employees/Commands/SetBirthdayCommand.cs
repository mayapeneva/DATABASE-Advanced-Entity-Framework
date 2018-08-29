namespace Employees.Commands
{
    using Data;
    using System;

    public class SetBirthdayCommand : AbstractCommand
    {
        public SetBirthdayCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var employeeId = int.Parse(arguments[0]);
            var birthday = DateTime.ParseExact(arguments[1], "dd-MM-yyyy", null);

            var employee = base.context.Employees.Find(employeeId);
            employee.Birthday = birthday;

            base.context.SaveChanges();

            return $"Birthday was addded successfully to employee {employee.FirstName} {employee.LastName}";
        }
    }
}