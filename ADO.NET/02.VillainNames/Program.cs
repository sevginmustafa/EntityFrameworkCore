using System;
using Microsoft.Data.SqlClient;

namespace _02.VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS; DATABASE = MinionsDB; Integrated Security = true");
            connection.Open();

            using (connection)
            {
                SqlCommand command = new SqlCommand(@"SELECT V.Name, COUNT(M.Id) AS Count FROM Villains V
                                                      JOIN MinionsVillains MV ON MV.VillainId = V.Id
                                                      JOIN Minions M ON M.Id = MV.MinionId
                                                      GROUP BY V.Name
                                                      HAVING COUNT(M.Id) >= 3
                                                      ORDER BY Count DESC", connection);

                var reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int count = (int)reader["Count"];

                        Console.WriteLine($"{name} - {count}");
                    }
                }
            }
        }
    }
}
