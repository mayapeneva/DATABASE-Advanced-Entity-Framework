namespace Employees
{
    using App.DTOs;
    using AutoMapper;
    using Models.Models;

    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            this.CreateMap<Employee, EmployeeDTO>().ReverseMap();
            this.CreateMap<Employee, EmployeeAllInfoDTO>().ReverseMap();
            this.CreateMap<Employee, ManagerDTO>().ReverseMap();
        }
    }
}