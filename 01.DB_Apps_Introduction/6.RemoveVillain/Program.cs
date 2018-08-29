using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _6.RemoveVillain
{
    public class Program
    {
        public static void Main()
        {
            var villainId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();
                connection.ChangeDatabase("Minions");

                string villainName;
                var commandText = "SELECT Name FROM Villains WHERE Id = @villainId";
                using (var command1 = new SqlCommand(commandText, connection))
                {
                    command1.Parameters.AddWithValue("@villainId", villainId);
                    if (command1.ExecuteScalar() == null)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }

                    villainName = (string)command1.ExecuteScalar();
                }

                int minionsReleased = 0;
                commandText = "DELETE MinionsVillains WHERE VillainId = @villainId";
                using (var command2 = new SqlCommand(commandText, connection))
                {
                    command2.Parameters.AddWithValue("@villainId", villainId);
                    minionsReleased = command2.ExecuteNonQuery();
                }

                commandText = "DELETE Villains WHERE Id = @villainId";
                using (var command3 = new SqlCommand(commandText, connection))
                {
                    command3.Parameters.AddWithValue("@villainId", villainId);
                    command3.ExecuteNonQuery();
                }

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{minionsReleased} minions were released.");

                connection.Close();
            }
        }
    }
}