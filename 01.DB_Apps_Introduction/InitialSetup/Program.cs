using System;
using System.Data.SqlClient;

namespace InitialSetup
{
    public class Program
    {
        public static void Main()
        {
            using (var connection = new SqlConnection(Constants.Connection))
            {
                connection.Open();

                var dataBase = "CREATE DATABASE Minions";
                ExecuteCommand(dataBase, connection);

                connection.ChangeDatabase("Minions");

                var countries = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";
                ExecuteCommand(countries, connection);

                var towns = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";
                ExecuteCommand(towns, connection);

                var minions = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";
                ExecuteCommand(minions, connection);

                var evilness = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";
                ExecuteCommand(evilness, connection);

                var villains =
                    "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";
                ExecuteCommand(villains, connection);

                var minionsvillains =
                    "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";
                ExecuteCommand(minionsvillains, connection);

                var countriesInsert =
                    "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";
                ExecuteCommand(countriesInsert, connection);

                var townsInsert =
                    "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";
                ExecuteCommand(townsInsert, connection);

                var minionsInsert =
                    "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";
                ExecuteCommand(minionsInsert, connection);

                var evilnessInsert =
                    "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";
                ExecuteCommand(evilnessInsert, connection);

                var villainsInsert =
                    "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";
                ExecuteCommand(villainsInsert, connection);

                var minionsvillainsInsert =
                    "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";
                ExecuteCommand(minionsvillainsInsert, connection);

                connection.Close();
            }
        }

        private static void ExecuteCommand(string commText, SqlConnection connection)
        {
            using (var command = new SqlCommand(commText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}