using System;
using System.Data.SqlClient;
using System.Linq;
using InitialSetup;

namespace _8.IncreaseMinionAge
{
    public class Program
    {
        public static void Main()
        {
            var minionIds = Console.ReadLine().Split().Select(int.Parse).ToArray();
            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();
                connection.ChangeDatabase("Minions");

                var commandText = "UPDATE Minions SET Name = UPPER(LEFT(Name, 1)) + LOWER(RIGHT(Name, LEN(Name) - 1)) WHERE ID = @Id";
                using (var command = new SqlCommand(commandText, connection))
                {
                    foreach (var minionId in minionIds)
                    {
                        command.Parameters.AddWithValue("@Id", minionId);
                        command.ExecuteNonQuery();
                    }
                }

                commandText = "UPDATE Minions SET Age += 1 WHERE ID IN(@Id)";
                using (var command2 = new SqlCommand(commandText, connection))
                {
                    foreach (var minionId in minionIds)
                    {
                        command2.Parameters.AddWithValue("Id", minionId);
                        command2.ExecuteNonQuery();
                    }
                }

                commandText = "SELECT Name, Age FROM Minions";
                using (var command3 = new SqlCommand(commandText, connection))
                {
                    using (var reader = command3.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}