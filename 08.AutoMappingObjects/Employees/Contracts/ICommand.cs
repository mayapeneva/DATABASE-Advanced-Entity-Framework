namespace Employees.Contracts
{
    public interface ICommand
    {
        string Execute(string[] arguments);
    }
}