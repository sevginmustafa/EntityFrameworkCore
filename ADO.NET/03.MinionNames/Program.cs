using System;
using Microsoft.Data.SqlClient;

namespace _03.MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS; Database=MinionsDB; Integrated Security=true");
            connection.Open();

            string inputId = Console.ReadLine();

            using (connection)
            {
                SqlCommand command = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = @inputId", connection);
                command.Parameters.AddWithValue("@inputId", inputId);

                string villainName = (string)command.ExecuteScalar();

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {inputId} exists in the database.");
                    return;
                }

                command = new SqlCommand($@"SELECT M.Name, M.Age FROM Villains V
                                           JOIN MinionsVillains MV ON MV.VillainId = V.Id
                                           JOIN Minions M ON M.Id = MV.MinionId
                                           WHERE V.Id = @inputId
                                           ORDER BY M.Name", connection);
                command.Parameters.AddWithValue("@inputId", inputId);

                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine($"Villain: {villainName}");

                using (reader)
                {
                    int counter = 1;

                    while (reader.Read())
                    {
                        string minionName = (string)reader["Name"];
                        int age = (int)reader["Age"];

                        Console.WriteLine($"{counter}. {minionName} {age}");

                        counter++;
                    }

                    if (counter == 1)
                    {
                        Console.WriteLine("(no minions)");
                    }
                }
            }
        }
    }
}
