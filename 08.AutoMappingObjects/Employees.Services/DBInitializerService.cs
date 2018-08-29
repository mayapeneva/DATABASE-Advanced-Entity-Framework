namespace Employees.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class DBInitializerService : IDBInitializerService
    {
        private EmployeesContext context;

        public DBInitializerService(EmployeesContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}