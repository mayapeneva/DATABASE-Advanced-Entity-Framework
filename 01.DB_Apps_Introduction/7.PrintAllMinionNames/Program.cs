using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InitialSetup;

namespace _7.PrintAllMinionNames
{
    public class Program
    {
        public static void Main()
        {
            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();
                connection.ChangeDatabase("Minions");

                var minions = new List<string>();
                var commandText = "SELECT Name FROM Minions";
                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add((string)reader[0]);
                        }
                    }
                }

                var counter1 = 0;
                var counter2 = minions.Count - 1;
                while (counter2 > counter1)
                {
                    Console.WriteLine(minions[counter1++]);
                    Console.WriteLine(minions[counter2--]);
                }
                if (counter2 % 2 != 0)
                {
                    Console.WriteLine(minions[counter1]);
                }

                connection.Close();
            }
        }
    }
}