using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _3.MinionNames
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
                var commText = "SELECT Name FROM Villains WHERE Id = @villainId";
                using (var command = new SqlCommand(commText, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);

                    villainName = (string)command.ExecuteScalar();
                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villainName}");
                }

                commText =
                    "SELECT m.Name, m.Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @villainId ORDER BY m.Name";
                using (var command2 = new SqlCommand(commText, connection))
                {
                    command2.Parameters.AddWithValue("@villainId", villainId);

                    var counter = 1;
                    using (var reader = command2.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                            return;
                        }

                        while (reader.Read())
                        {
                            Console.WriteLine($"{counter++}. {reader["Name"]} - {reader["Age"]}");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}