using System;
using System.Data.SqlClient;
using System.Linq;
using InitialSetup;

namespace _4.AddMinion
{
    public class Program
    {
        public static void Main()
        {
            var minionInfo = Console.ReadLine().Split().ToArray();
            var minionName = minionInfo[1];
            var minionAge = int.Parse(minionInfo[2]);
            var minionTown = minionInfo[3];

            var villainInfo = Console.ReadLine().Split().ToArray();
            var villainName = villainInfo[1];

            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();
                connection.ChangeDatabase("Minions");

                var townId = 0;
                var commandText = "SELECT Id FROM Towns WHERE Name = @town";
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@town", minionTown);
                    if (command.ExecuteScalar() == null)
                    {
                        commandText = "INSERT INTO Towns (Name) VALUES (@town)";
                        using (var command2 = new SqlCommand(commandText, connection))
                        {
                            command2.Parameters.AddWithValue("@town", minionTown);
                            command2.ExecuteNonQuery();

                            Console.WriteLine($"Town {minionTown} was added to the database.");
                        }
                    }

                    townId = (int)command.ExecuteScalar();
                }

                commandText = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
                using (var command3 = new SqlCommand(commandText, connection))
                {
                    command3.Parameters.AddWithValue("@name", minionName);
                    command3.Parameters.AddWithValue("@age", minionAge);
                    command3.Parameters.AddWithValue("@townId", townId);

                    command3.ExecuteNonQuery();
                }

                var villainId = 0;
                commandText = "SELECT Id FROM Villains WHERE Name = @name";
                using (var command4 = new SqlCommand(commandText, connection))
                {
                    command4.Parameters.AddWithValue("@name", villainName);
                    if (command4.ExecuteScalar() == null)
                    {
                        commandText = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@name, 4)";
                        using (var command5 = new SqlCommand(commandText, connection))
                        {
                            command5.Parameters.AddWithValue("@name", villainName);
                            command5.ExecuteNonQuery();

                            Console.WriteLine($"Villain {villainName} was added to the database.");
                        }
                    }

                    villainId = (int)command4.ExecuteScalar();
                }

                var minionId = 0;
                commandText = "SELECT Id FROM Minions WHERE Name = @name";
                using (var command6 = new SqlCommand(commandText, connection))
                {
                    command6.Parameters.AddWithValue("@name", minionName);

                    minionId = (int)command6.ExecuteScalar();
                }

                commandText = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@mId, @vId)";
                using (var command7 = new SqlCommand(commandText, connection))
                {
                    command7.Parameters.AddWithValue("@mId", minionId);
                    command7.Parameters.AddWithValue("@vId", villainId);

                    command7.ExecuteNonQuery();
                }

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");

                connection.Close();
            }
        }
    }
}