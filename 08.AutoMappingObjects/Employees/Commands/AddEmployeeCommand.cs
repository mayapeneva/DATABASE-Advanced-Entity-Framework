namespace Employees.Commands
{
    using App.DTOs;
    using AutoMapper;
    using Data;
    using Models.Models;

    public class AddEmployeeCommand : AbstractCommand
    {
        private readonly IMapper mapper;

        public AddEmployeeCommand(EmployeesContext context, IMapper mapper)
            : base(context)
        {
            this.mapper = mapper;
        }

        public override string Execute(string[] arguments)
        {
            var firstName = arguments[0];
            var lastName = arguments[1];
            var salary = decimal.Parse(arguments[2]);

            var employeeDto = new EmployeeDTO()
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            var employee = this.mapper.Map<Employee>(employeeDto);

            base.context.Employees.Add(employee);
            base.context.SaveChanges();

            return $"Employee {firstName} {lastName} was addded successfully";
        }
    }
}