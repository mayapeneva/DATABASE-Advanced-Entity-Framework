using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InitialSetup;

namespace _5.ChangeTownNamesCasing
{
    public class Program
    {
        public static void Main()
        {
            var country = Console.ReadLine();

            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();
                connection.ChangeDatabase("Minions");

                var commandText =
                    "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = (SELECT Id FROM Countries WHERE Name = @country)";
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@country", country);
                    var rowsNumber = command.ExecuteNonQuery();
                    if (rowsNumber == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                        return;
                    }

                    Console.WriteLine($"{rowsNumber} town names were affected.");
                }

                var towns = new List<string>();
                commandText = "SELECT Name FROM Towns WHERE CountryCode = (SELECT Id FROM Countries WHERE Name = @country)";
                using (var command2 = new SqlCommand(commandText, connection))
                {
                    command2.Parameters.AddWithValue("@country", country);
                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            towns.Add((string)reader[0]);
                        }
                    }
                }

                Console.WriteLine($"[{string.Join(", ", towns)}]");

                connection.Close();
            }
        }
    }
}