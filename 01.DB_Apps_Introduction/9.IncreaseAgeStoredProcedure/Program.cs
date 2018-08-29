using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _9.IncreaseAgeStoredProcedure
{
    public class Program
    {
        public static void Main()
        {
            var minionId = int.Parse(Console.ReadLine());
            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();
                connection.ChangeDatabase("Minions");

                var commandText =
                    "CREATE PROCEDURE usp_GetOlder(@minionId INT) AS BEGIN UPDATE Minions SET Age += 1 WHERE Id = @minionId END";
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }

                commandText = "EXECUTE usp_GetOlder @minionId";
                using (var command2 = new SqlCommand(commandText, connection))
                {
                    command2.Parameters.AddWithValue("@minionId", minionId);
                    command2.ExecuteNonQuery();
                }

                commandText = "SELECT Name, Age FROM Minions";
                using (var command3 = new SqlCommand(commandText, connection))
                {
                    using (var reader = command3.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}