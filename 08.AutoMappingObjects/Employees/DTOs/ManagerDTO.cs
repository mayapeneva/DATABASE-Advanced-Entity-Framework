namespace Employees.App.DTOs
{
    using System.Collections.Generic;

    public class ManagerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<EmployeeDTO> EmployeesToManage { get; set; } = new HashSet<EmployeeDTO>();
    }
}