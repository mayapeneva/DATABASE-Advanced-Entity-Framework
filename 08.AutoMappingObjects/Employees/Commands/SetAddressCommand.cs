namespace Employees.Commands
{
    using Data;
    using System.Linq;

    public class SetAddressCommand : AbstractCommand
    {
        public SetAddressCommand(EmployeesContext context)
            : base(context)
        {
        }

        public override string Execute(string[] arguments)
        {
            var employeeId = int.Parse(arguments[0]);
            var address = string.Join(" ", arguments.Skip(1));

            var employee = base.context.Employees.Find(employeeId);
            employee.Address = address;

            base.context.SaveChanges();

            return $"Address was addded successfully to employee {employee.FirstName} {employee.LastName}";
        }
    }
}