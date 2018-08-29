namespace Employees.Commands
{
    using Contracts;
    using Data;

    public abstract class AbstractCommand : ICommand
    {
        protected readonly EmployeesContext context;

        protected AbstractCommand(EmployeesContext context)
        {
            this.context = context;
        }

        public abstract string Execute(string[] arguments);
    }
}