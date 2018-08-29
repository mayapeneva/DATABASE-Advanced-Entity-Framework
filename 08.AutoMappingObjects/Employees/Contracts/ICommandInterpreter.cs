namespace Employees.Contracts
{
    public interface ICommandInterpreter
    {
        ICommand Interpret(string command);
    }
}