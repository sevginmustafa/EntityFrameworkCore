using Microsoft.Data.SqlClient;
using System;

namespace _09.IncreaseAgeStoredProcedure
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
                //Procedure should look like this:
                  //CREATE PROC usp_GetOlder(@MinionId INT)
                  //AS
                  //    UPDATE Minions SET Age += 1
                  //    WHERE Id = @MinionId

                SqlCommand commandUpdate = new SqlCommand(@"EXEC usp_GetOlder @minionId", connection);
                commandUpdate.Parameters.AddWithValue("@minionId", inputId);

                commandUpdate.ExecuteNonQuery();


                SqlCommand commandPrint = new SqlCommand(@"SELECT Name, Age FROM Minions WHERE Id = @minionId", connection);
                commandPrint.Parameters.AddWithValue("@minionId", inputId);

                SqlDataReader reader = commandPrint.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int age = (int)reader["Age"];

                        Console.WriteLine($"{name} – {age} years old");
                    }
                }
            }
        }
    }
}
