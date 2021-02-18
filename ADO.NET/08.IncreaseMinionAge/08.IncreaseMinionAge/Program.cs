using Microsoft.Data.SqlClient;
using System;

namespace _08.IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS; Database = MinionsDB; Integrated Security=true");
            connection.Open();

            string[] minionIDs = Console.ReadLine().Split();

            using (connection)
            {
                for (int i = 0; i < minionIDs.Length; i++)
                {
                    SqlCommand commandUpdate = new SqlCommand(@"UPDATE Minions 
                                                                SET Name = UPPER(LEFT(Name, 1)) + (LOWER(RIGHT(Name, LEN(Name) - 1))), Age += 1
                                                                WHERE Id = @minionId", connection);
                    commandUpdate.Parameters.AddWithValue("@minionId", minionIDs[i]);

                    commandUpdate.ExecuteNonQuery();
                }

                SqlCommand commandPrint = new SqlCommand(@"SELECT Name, Age FROM Minions", connection);

                SqlDataReader reader = commandPrint.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int age = (int)reader["Age"];

                        Console.WriteLine($"{name} {age}");
                    }
                }
            }
        }
    }
}
