namespace Employees.App.DTOs
{
    public class EmployeeWithManagerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public ManagerDTO Manager { get; set; }
    }
}