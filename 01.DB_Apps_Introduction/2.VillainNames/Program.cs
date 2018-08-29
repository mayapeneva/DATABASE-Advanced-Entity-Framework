using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _2.VillainNames
{
    public class Program
    {
        public static void Main()
        {
            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();

                connection.ChangeDatabase("Minions");
                var command = new SqlCommand("SELECT Name, COUNT(mv.MinionId) AS MinionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Name HAVING COUNT(mv.MinionId) > 3 ORDER BY MinionsCount", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                    }
                }

                connection.Close();
            }
        }
    }
}